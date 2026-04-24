using UnityEngine;

public class PlayerIdleState : PlayerState
{
    public PlayerIdleState(Player player) : base(player, PlayerStateType.IDLE) {}

    public override void Update(PlayerInputSnapshot inputSnapshot)
    {
        SetMoveDirection(inputSnapshot);
        SetLookDirection(inputSnapshot);
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

        if (inputSnapshot.move.IsNotNearlyZero())
        {
            Player.StateMachine.ChangeState(PlayerStateType.MOVE);
            return;
        }
    }
}
