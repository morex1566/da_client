using UnityEngine;
using UnityEngine.InputSystem;

public static class Utls
{
    // Current, Parent, Children 전체 시도
    public static T FindComponent<T>(GameObject obj) where T : Component
    {
        T comp = obj.GetComponent<T>();
        if (comp != null) return comp;
            
        comp = obj.GetComponentInParent<T>();
        if (comp != null) return comp;

        return obj.GetComponentInChildren<T>();
    }

    public static bool IsNearlyZero(this Vector2 value)
    {
        return value.sqrMagnitude < 0.0001f;
    }

    public static bool IsNotNearlyZero(this Vector2 value)
    {
        return value.sqrMagnitude >= 0.0001f;
    }

    public static Vector3 GetMouseWorldPosition(Camera camera = null)
    {
        if (camera == null) camera = Camera.main;

        Vector3 screenPos = Mouse.current.position.ReadValue();
        screenPos.z = -camera.transform.position.z;

        Vector3 worldPos = camera.ScreenToWorldPoint(screenPos);
        worldPos.z = 0f;

        return worldPos;
    }

    public static T FindComponentByTag<T>(string tag) where T : Component
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(tag);

        for (int i = 0; i < gameObjects.Length; i++)
        {
            T component = gameObjects[i].GetComponent<T>();

            if (component != null)
                return component;

            component = gameObjects[i].GetComponentInChildren<T>();

            if (component != null)
                return component;

            component = gameObjects[i].GetComponentInParent<T>();

            if (component != null)
                return component;
        }

        return null;
    }
}