using UnityEngine;

public class PlayerMoveState : PlayerState
{
    public PlayerMoveState(Player player) : base(player, PlayerStateType.MOVE) {}

    public override void Update(PlayerInputSnapshot inputSnapshot)
    {
        SetMoveDirection(inputSnapshot);
        SetLookDirection(inputSnapshot);
        Move(inputSnapshot);
    }

    public override void Evaluate(PlayerInputSnapshot inputSnapshot)
    {
        if (Player.CurrentHp <= 0f)
        {
            Player.StateMachine.ChangeState(PlayerStateType.DEAD);
            return;
        }

        if (inputSnapshot.rollPressed)
        {
            Player.StateMachine.ChangeState(PlayerStateType.ROLL);
            return;
        }

        if (inputSnapshot.jumpPressed)
        {
            Player.StateMachine.ChangeState(PlayerStateType.Jump);
            return;
        }

        if (inputSnapshot.move.IsNearlyZero())
        {
            Player.StateMachine.ChangeState(PlayerStateType.IDLE);
            return;
        }
    }
}
