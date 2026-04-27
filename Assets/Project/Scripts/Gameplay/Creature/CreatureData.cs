using UnityEngine;

public abstract class CreatureData : ScriptableObject
{
    public Vector3 MaxSpeed = new Vector3(3f, 0f, 3f);
    public float MaxHp = 100f;
    public float MaxSp = 100f;
}
