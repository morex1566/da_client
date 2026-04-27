using UnityEngine;

public class PlayerIdleState : PlayerState
{
    public PlayerIdleState(PlayerController controller) : base(controller, PlayerStateType.IDLE) {}

    public override void Update(InputSnapshot inputSnapshot)
    {
        SetMoveDirection(inputSnapshot);
        SetLookDirection(inputSnapshot);
    }

    public override PlayerStateType? Evaluate(InputSnapshot inputSnapshot)
    {
        if (Controller.CurrentHp <= 0f)
        {
            return PlayerStateType.DEAD; // 체력이 없으면 모든 입력보다 사망 상태 우선
        }

        if (inputSnapshot.rollPressed)
        {
            return PlayerStateType.ROLL; // 구르기 입력은 정지 상태에서 즉시 반영
        }

        if (inputSnapshot.move.IsNotNearlyZero())
        {
            return PlayerStateType.MOVE; // 이동 입력이 들어오면 이동 상태로 전이
        }

        return null;
    }
}
