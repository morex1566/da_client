#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

public static class ScriptExecutionOrder
{
    [MenuItem("Tools/Script Execution Order/Apply")]
    private static void ApplyPlayerOrder()
    {
        Set<Player>(1000);
        Set<PlayerController>(1199);
        Set<PlayerView>(1299);

        Debug.Log("[ScriptExecutionOrder] Applied: PlayerController=1000, PlayerView=1999");
    }

    private static void Set<T>(int order) where T : MonoBehaviour
    {
        MonoScript monoScript = FindMonoScript(typeof(T));
        if (monoScript == null)
        {
            Debug.LogError($"[ScriptExecutionOrder] MonoScript not found: {typeof(T).FullName}");
            return;
        }

        MonoImporter.SetExecutionOrder(monoScript, order);
    }

    private static MonoScript FindMonoScript(Type type)
    {
        MonoScript[] scripts = MonoImporter.GetAllRuntimeMonoScripts();
        for (int i = 0; i < scripts.Length; i++)
        {
            MonoScript script = scripts[i];
            if (script == null) continue;

            if (script.GetClass() == type)
            {
                return script;
            }
        }

        return null;
    }
}
#endif

