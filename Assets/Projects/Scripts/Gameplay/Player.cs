using UnityEngine;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(PlayerView))]
[DisallowMultipleComponent]
public class Player : Creature
{
    public bool IsRolling { get; set; } = false;

    public bool IsGroggy { get; set; } = false;

    public Vector2 MoveDirection { get; set; } = Vector2.zero;

    public Vector2 LookDirection { get; set; } = Vector2.right;






    private void OnValidate()
    {
        Init();
    }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Init()
    {
        base.Init();
    }
}
