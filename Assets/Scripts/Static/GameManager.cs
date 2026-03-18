using NetCore;
using UnityEngine;
using Common;

#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
[InitializeOnLoad]
#endif
public class GameManager : MonoBehaviourSingleton<GameManager>
{
    private static NetManager netManagerInstance;

#if UNITY_EDITOR
    [MenuItem("Network/Init")]
#endif
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void OnAfterSceneLoaded()
    {
        netManagerInstance = NetManager.GetInstance();
        {
            netManagerInstance.Init();
        }
    }
}