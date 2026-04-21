using System;
using UnityEngine;

public abstract class Creature : MonoBehaviour
{
    [field: SerializeField] public CreatureData Data { get; set; } = null;

    public float CurrentHp { get; set; } = 0f;

    public float CurrentSp { get; set; } = 0f;

    public bool IsDead { get; set; } = false;

    public bool IsMoving { get; set; } = false;

    public bool IsLeft { get; set; } = false;






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
