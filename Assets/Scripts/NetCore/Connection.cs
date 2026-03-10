using NetCommon;
using NetPacket;
using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;


namespace NetCore
{
    public class Connection : IDisposable
    {
        public Connection(IOContext context, Socket socket, int connectionId)
        {
            _context = context;
            _socket = socket;
            _connectionId = connectionId;

            _readHeaderEvt = new SocketAsyncEventArgs();
            {
                _readHeaderEvt.Completed += OnAsyncReadHeaderCompleted;
            }

            _readPayloadEvt = new SocketAsyncEventArgs();
            {
                _readPayloadEvt.Completed += OnAsyncReadPayloadCompleted;
            }

            _writeEvt = new SocketAsyncEventArgs();
            {
                _writeEvt.Completed += OnAsyncWriteCompleted;
            }
        }

        public void Init()
        {

        }

        public void AsyncRead()
        {
            // 큐잉 한계
            if (_recvQueue.IsAddingCompleted)
            {
                return;
            }

            AsyncReadHeader();
        }

        public void AsyncWrite()
        {
            // 쓰고 있음
            if (_isWriting == 1)
            {
                return;
            }

            FlushBatch();
        }

        private void AsyncReadHeader()
        {
            int headerSize = Marshal.SizeOf<PacketHeader>();

            /// <summary>
            /// 헤더 수신 시작. Partial Read 대응을 위해 Memory.Slice 사용
            /// </summary>
            if (_recvBytes <= 0)
            {
                _cachedHeaderOwner = MemoryPool<byte>.Shared.Rent(headerSize);
                _readHeaderEvt.SetBuffer(_cachedHeaderOwner.Memory);
            }
            else
            {
                int remainBytes = headerSize - _recvBytes;
                _readHeaderEvt.SetBuffer(_cachedHeaderOwner.Memory.Slice(_recvBytes, remainBytes));
            }

            _socket.ReceiveAsync(_readHeaderEvt);
        }

        private unsafe void AsyncReadPayload()
        {
            int payloadSize = 0;
            fixed (byte* ptr = &_cachedHeaderOwner.Memory.Span[0])
            {
                PacketHeader header = *(PacketHeader*)ptr;
                payloadSize = header.payloadSize;
            }

            /// <summary>
            /// 페이로드 수신 시작. 헤더에서 추출한 사이즈만큼 할당 및 수신
            /// </summary>
            if (_recvBytes <= 0)
            {
                _cachedPayloadOwner = MemoryPool<byte>.Shared.Rent(payloadSize);
                _readPayloadEvt.SetBuffer(_cachedPayloadOwner.Memory);
            }
            else
            {

                int remainBytes = payloadSize - _recvBytes;
                _readPayloadEvt.SetBuffer(_cachedPayloadOwner.Memory.Slice(_recvBytes, remainBytes));
            }

            _socket.ReceiveAsync(_readPayloadEvt);
        }

        private void FlushBatch()
        {
            // 이미 송신 중이면 중복 진입 방지
            if (Interlocked.CompareExchange(ref _isWriting, 1, 0) != 0)
            {
                return;
            }

            // 패킷을 Threshold만큼 큐에서 추출하여 하나의 BufferList로 구성
            while (_cachedDisposeBuffer.Count < SendBatchThreshold && _sendQueue.TryTake(out var packetMemoryOwner))
            {
                if (MemoryMarshal.TryGetArray<byte>(packetMemoryOwner.Header.Memory, out var header))
                {
                    _cachedSendBuffer.Add(header);
                }

                if (packetMemoryOwner.Payload != null && MemoryMarshal.TryGetArray<byte>(packetMemoryOwner.Payload.Memory, out var payload))
                {
                    _cachedSendBuffer.Add(payload);
                }

                _cachedDisposeBuffer.Add(packetMemoryOwner);
            }

            // 전송할 데이터가 하나도 없을 때,
            if (_cachedDisposeBuffer.Count == 0)
            {
                Interlocked.Exchange(ref _isWriting, 0);
                return;
            }

            // 데이터 전송 시작
            _writeEvt.BufferList = _cachedSendBuffer;
            _writeEvt.UserToken = _cachedDisposeBuffer;

            _socket.SendAsync(_writeEvt);
        }

        public void Dispose()
        {
            if (_socket != null)
            {
                _socket.Close();
                _socket.Dispose();
            }

            if (_readHeaderEvt != null)
            {
                _readHeaderEvt.Dispose();
            }

            if (_readPayloadEvt != null)
            {
                _readPayloadEvt.Dispose();
            }

            _cachedHeaderOwner?.Dispose();
            _cachedPayloadOwner?.Dispose();
        }

        private void OnAsyncReadHeaderCompleted(object sender, SocketAsyncEventArgs evt)
        {
            if (evt.SocketError != SocketError.Success || evt.BytesTransferred == 0)
            {
                // 소켓 오류 또는 연결 종료 처리
                return;
            }

            // 들어온 데이터 저장
            _recvBytes += evt.BytesTransferred;

            // 아직 데이터 덜 들어옴
            int headerSize = Marshal.SizeOf<PacketHeader>();
            if (_recvBytes < headerSize)
            {
                AsyncReadHeader();
                return;
            }

            // 데이터 다 들어옴
            _recvBytes = 0;

            AsyncReadPayload();
        }

        private unsafe void OnAsyncReadPayloadCompleted(object sender, SocketAsyncEventArgs evt)
        {
            if (evt.SocketError != SocketError.Success || evt.BytesTransferred == 0)
            {
                // 소켓 오류 또는 연결 종료 처리
                return;
            }

            // 들어온 데이터 저장
            _recvBytes += evt.BytesTransferred;

            int payloadSize = 0;
            fixed (byte* ptr = &_cachedHeaderOwner.Memory.Span[0])
            {
                PacketHeader header = *(PacketHeader*)ptr;
                payloadSize = header.payloadSize;
            }

            // 아직 데이터 덜 들어옴
            if (_recvBytes < payloadSize)
            {
                AsyncReadPayload();
                return;
            }

            // 데이터 다 들어옴
            _recvBytes = 0;

            OnAsyncReadCompleted(sender, evt);
        }

        private void OnAsyncWriteCompleted(object sender, SocketAsyncEventArgs evt)
        {
            // 전송 완료된 패킷 메모리 해제
            if (evt.UserToken is List<PacketMemoryOwner> packets)
            {
                foreach (var packet in packets)
                {
                    packet.Header.Dispose();
                    packet.Payload?.Dispose();
                }
            }

            // 이벤트 캐시 초기화 + 쓰기 상태 해제
            _cachedSendBuffer.Clear();
            _cachedDisposeBuffer.Clear();

            Interlocked.Exchange(ref _isWriting, 0);
        }

        private void OnAsyncReadCompleted(object sender, SocketAsyncEventArgs evt)
        {
            if (!_recvQueue.TryAdd(new PacketMemoryOwner
            {
                Header = _cachedHeaderOwner,
                Payload = _cachedPayloadOwner
            }))
            {
                Log.Trace("recv queue is full.");
            }

            _cachedHeaderOwner = null;
            _cachedPayloadOwner = null;

            // 다시 새로운 패킷 받게 설정
            AsyncReadHeader();
        }



        public static readonly int MaxReceiveQueueSize = 1000;

        public static readonly int MaxSendQueueSize = 1000;

        public static readonly int SendBatchInterval = 50; // ms

        public static readonly int SendBatchThreshold = (int)(MaxSendQueueSize * 0.01);

        private readonly IOContext _context;

        private readonly Socket _socket;

        private readonly int _connectionId;

        private BlockingCollection<PacketMemoryOwner> _recvQueue = new(MaxReceiveQueueSize);

        private BlockingCollection<PacketMemoryOwner> _sendQueue = new(MaxSendQueueSize);

        public BlockingCollection<PacketMemoryOwner> RecvQueue { get { return _recvQueue; } }

        public BlockingCollection<PacketMemoryOwner> SendQueue { get { return _sendQueue; } }

        #region Reading
        private SocketAsyncEventArgs _readHeaderEvt, _readPayloadEvt;

        /// <summary>
        /// async read를 통해 읽어들인 메모리 span
        /// </summary>
        private IMemoryOwner<byte> _cachedHeaderOwner, _cachedPayloadOwner;

        /// <summary>
        /// async read에서 분할로 데이터가 들어왔을 경우 대비
        /// </summary>
        private int _recvBytes;
        #endregion

        #region Writing
        private SocketAsyncEventArgs _writeEvt;

        /// <summary>
        /// async write에서 sendqueue의 내용을 하나로 모은 것
        /// </summary>
        private List<ArraySegment<byte>> _cachedSendBuffer = new();

        private List<PacketMemoryOwner> _cachedDisposeBuffer = new();

        /// <summary>
        /// async write 조건
        /// </summary>
        private int _isWriting = 0;

        #endregion
    }
}