using System;
using UnityEngine;

public abstract class Creature : MonoBehaviour
{
    [SerializeField] protected CreatureData data = null;

    public CreatureData Data => data;

    public float CurrentHp { get; set; }

    public float CurrentSp { get; set; }

    public bool IsDead { get; set; }

    public bool IsMoving { get; set; }

    public bool IsLeft { get; set; }



    protected virtual void Awake()
    {
        Init();
    }

    public virtual void Init()
    {
        CurrentHp = data.MaxHp;
        CurrentSp = data.MaxSp;
    }
}
