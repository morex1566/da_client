using Common;
using NetCore;
using Google.FlatBuffers;

public enum NetProtocol
{
    TCP,
    UDP
}

public class NetManager : MonoBehaviourSingleton<NetManager>
{
    private readonly string Host = "192.168.0.3";

    private readonly int TcpPort = 60000;

    private IOContext ioContext;

    private IOContext.WorkGuard workGuard;

    private TCP tcp;



    public void Init()
    {
        ioContext = IOContext.GetInstance();
        {
            workGuard = ioContext.MakeWorkGuard();
            ioContext.Run();
        }

        tcp = TCP.GetInstance();
        {
            tcp.Init(ioContext, Host, TcpPort);
            tcp.AsyncConnect();
        }
    }
}
