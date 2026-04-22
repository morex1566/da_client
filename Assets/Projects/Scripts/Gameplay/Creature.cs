using System;
using UnityEngine;

public enum CreatureStateAttribute
{
    IsLeft,
    IsReload
}

public abstract class Creature : MonoBehaviour
{
    [field: SerializeField] public CreatureData Data { get; set; } = null;

    public float CurrentHp { get; set; } = 0f;

    public float CurrentSp { get; set; } = 0f;

    public bool IsLeft { get; set; } = false;


    protected virtual void OnValidate()
    {
        Init();
    }

    protected virtual void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {
        CurrentHp = Data.MaxHp;
        CurrentSp = Data.MaxSp;
    }
}
