using UnityEngine;

public class PlayerMoveState : PlayerState
{
    public PlayerMoveState(PlayerController controller) : base(controller, PlayerStateType.MOVE) {}

    public override void Update(InputSnapshot inputSnapshot)
    {
        SetMoveDirection(inputSnapshot);
        SetLookDirection(inputSnapshot);
        Move(inputSnapshot);
    }

    public override PlayerStateType? Evaluate(InputSnapshot inputSnapshot)
    {
        if (Controller.CurrentHp <= 0f)
        {
            return PlayerStateType.DEAD; // 체력이 없으면 이동보다 사망 상태 우선
        }

        if (inputSnapshot.rollPressed)
        {
            return PlayerStateType.ROLL; // 이동 중 구르기 입력을 우선 처리
        }

        if (inputSnapshot.move.IsNearlyZero())
        {
            return PlayerStateType.IDLE; // 이동 입력이 끊기면 대기 상태로 복귀
        }

        return null;
    }
}
