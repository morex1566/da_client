using NetPacket;
using NetCommon;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace NetCore
{
    /// <summary>
    /// IOContext 기반 비동기 TCP 클라이언트.
    /// Connect / Receive / AsyncSend 완료 콜백을 IOContext.Post() 로 전달.
    /// </summary>
    public class TCP : Singleton<TCP>
    {
        /// <summary>
        /// Default constructor for Singleton
        /// </summary>
        public TCP()
        {

        }

        /// <summary>
        /// TCP Instance Initialization
        /// </summary>
        public void Init(IOContext context, string host, int port)
        {
            _context = context;
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _endpoint = new IPEndPoint(IPAddress.Parse(host), port);
        }

        /// <summary>
        /// 서버에 비동기 연결 시도. 완료 시 IOContext.Post() 를 통해 onConnected 콜백 실행
        /// </summary>
        public void AsyncConnect()
        {
            Debug.Log("Start to AsyncConnect");

            _socket.ConnectAsync(_endpoint).ContinueWith((Task task) =>
            {
                _context.Dispatch(() =>
                {
                    // 서버 연결 성공
                    if (task.IsCompletedSuccessfully)
                    {
                        OnConnectComplete();
                    }
                    // 서버 연결 실패
                    else
                    {
                        OnConnectFailed();
                    }
                });
            });
        }

        /// <summary>
        /// 소켓 닫기
        /// </summary>
        public void Disconnect()
        {
            if (_socket == null)
            {
                return;
            }
           
            _socket.Close();
        }

        public void AsyncSend(PacketType type, Google.Protobuf.IMessage msg)
        {
            var packetMemoryOwner = PacketUtility.Serialize(type, msg);

            // AsyncSend 한계 확인
            if (!_connection.SendQueue.TryAdd(packetMemoryOwner))
            {
                Log.Trace("send queue is overflowed.");
                return;
            }

            // 쓰기 실행
            _connection.AsyncWrite();
        }

        public void AsyncReceive()
        {

        }

        /// <summary>
        /// 해당 서버와 연결된 Connection 클래스 생성
        /// </summary>
        private void OnConnectComplete()
        {
            // 연결 고유 id 받아옴
            int currConnectionId = Volatile.Read(ref _connectionId);
            Volatile.Write(ref _connectionId, _connectionId + 1);

            // [수정] Connection 생성 시 세그먼트 풀 전달
            _connection = new Connection(_context, _socket, currConnectionId);

            Debug.Log("Connection Completed.");
        }

        private void OnConnectFailed()
        {
            Debug.Log("Connection Failed.");
        }


        private IOContext _context;

        private Socket _socket;

        private IPEndPoint _endpoint;

        private Connection _connection;

        private int _connectionId;

        public bool IsConnected => _socket != null && _socket.Connected;
    }
}
