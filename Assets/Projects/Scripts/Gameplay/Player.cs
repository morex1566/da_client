using UnityEngine;

public class Player : Creature
{
    public bool IsRolling { get; set; }

    public bool IsGroggy { get; set; }

    public Vector2 MoveDirection { get; set; } = Vector2.zero;

    public Vector2 LookDirection { get; set; } = Vector2.right;
}
