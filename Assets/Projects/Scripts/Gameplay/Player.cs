using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(PlayerView))]
[DisallowMultipleComponent]
public class Player : Creature
{
    public PlayerStateMachine StateMachine { get; private set; } = null;

    public Vector2 MoveDirection { get; set; } = Vector2.zero;

    public Vector2 LookDirection { get; set; } = Vector2.right;

    public bool IsRollRequested { get; set; } = false;

    public bool IsGroggyRequested { get; set; } = false;

    public bool IsDeadRequested { get; set; } = false;




    protected override void OnValidate()
    {
        base.OnValidate();

        Init();
    }

    protected override void Awake()
    {
        base.Awake();

        Init();
    }

    protected override void Init()
    {
        base.Init();

        StateMachine = new(this);
    }
}
