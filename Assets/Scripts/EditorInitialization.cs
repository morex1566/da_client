using UnityEngine;
using NetCore;
#if UNITY_EDITOR
using UnityEditor;

[InitializeOnLoad]
public static class EditorInitialization
{
    private static IOContext ioContext;

    private static IOContext.WorkGuard workGuard;

    public static TCP tcp;

    static EditorInitialization()
    {
        InitNetworkSystem();

        EditorApplication.update += OnEditorUpdate;
    }

    [MenuItem("Network/Init")]
    private static void InitNetworkSystem()
    {
        ioContext = IOContext.GetInstance();
        {
            workGuard = ioContext.MakeWorkGuard();
            ioContext.Run();
        }

        tcp = TCP.GetInstance();
        {
            tcp.Init(ioContext, NetConfig.host, NetConfig.tcpPort);
            tcp.AsyncConnect();
        }
    }

    [MenuItem("Network/Connect")]
    private static void Connect()
    {
        tcp.AsyncConnect();
    }

    [MenuItem("Network/Disconnect")]
    private static void Disconnect()
    {
        tcp.Disconnect();
    }

    private static void OnEditorUpdate()
    {

    }
}

#endif