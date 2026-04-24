using UnityEngine;

public class PlayerRollState : PlayerState
{
    private Animator animator;

    private float moveBlockTime = 0.275f;

    public PlayerRollState(Player player) : base(player, PlayerStateType.ROLL) 
    {
        animator = Utls.FindComponent<Animator>(player.gameObject);
    }

    public override void Update(PlayerInputSnapshot inputSnapshot)
    {
        SetMoveDirection(inputSnapshot);
        Move(inputSnapshot);
    }

    protected override void Move(PlayerInputSnapshot inputSnapshot)
    {
        if (inputSnapshot.move.IsNearlyZero()) return;

        Vector3 frameVelocity = new Vector3
        (
            Player.MoveDirection.x * Player.Data.MaxSpeed.x,
            0,
            Player.MoveDirection.z * Player.Data.MaxSpeed.z
        );

        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);

        // 구르기 초기에는 빠르게 이동
        if (info.IsName(UnityConstant.Animator.Parameters.AC_Player.Bool.IsRoll) && info.normalizedTime <= moveBlockTime)
        {
            Player.transform.position += frameVelocity * Player.Data.SpeedAccelAtRollMultiplier * Time.deltaTime;

        }

        // 구르기 다 끝나가면 느리게 이동
        if (info.IsName(UnityConstant.Animator.Parameters.AC_Player.Bool.IsRoll) && info.normalizedTime > moveBlockTime)
        {
            Player.transform.position += frameVelocity * Player.Data.SpeedDecelAtRollMultiplier * Time.deltaTime;
        }
    }

    // 애니메이션 마지막 즈음엔 살짝 움직일 수 있게
    protected override void SetMoveDirection(PlayerInputSnapshot inputSnapshot)
    {
        if (inputSnapshot.move.IsNearlyZero()) return;

        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
        if (info.IsName(UnityConstant.Animator.Parameters.AC_Player.Bool.IsRoll) && info.normalizedTime <= moveBlockTime) return;

        Vector3 input = inputSnapshot.move.normalized;
        Player.MoveDirection = Vector3.Lerp(Player.MoveDirection, new Vector3(input.x, 0, input.y), Time.deltaTime * 10);
    }

    public override void Evaluate(PlayerInputSnapshot inputSnapshot)
    {
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);

        // 구르기 끝났는지?
        if (info.IsName(UnityConstant.Animator.Parameters.AC_Player.Bool.IsRoll) && info.normalizedTime >= 0.9f)
        {
            Player.StateMachine.ChangeState(inputSnapshot.move.IsNearlyZero() ? PlayerStateType.IDLE : PlayerStateType.MOVE);
            return;
        }
    }
}
