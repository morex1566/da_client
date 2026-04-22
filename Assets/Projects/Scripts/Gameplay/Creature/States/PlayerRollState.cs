using UnityEngine;

public class PlayerRollState : PlayerState
{
    private Animator animator;

    public PlayerRollState(Player player) : base(player, PlayerStateType.ROLL) 
    {
        animator = Utls.FindComponent<Animator>(player.gameObject);
    }

    public override void Update(PlayerInputSnapshot inputSnapshot)
    {
        SetMoveDirection(inputSnapshot);
        Move();
    }

    private void Move()
    {
        Vector2 frameVelocity = new Vector2
        (
            Player.MoveDirection.x * Player.Data.MaxSpeed.x,
            Player.MoveDirection.y * Player.Data.MaxSpeed.y
        );

        Player.transform.position += (Vector3)frameVelocity * Time.deltaTime;
    }

    // 애니메이션 마지막 즈음엔 살짝 움직일 수 있게?
    private void SetMoveDirection(PlayerInputSnapshot inputSnapshot)
    {
        //AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);

        //if (info.IsName(UnityConstant.Animator.Parameters.AC_Player.Bool.IsRoll) && info.normalizedTime <= 0.9f) return;

        //Player.MoveDirection = inputSnapshot.move.normalized;
    }

    public override void Evaluate(PlayerInputSnapshot input)
    {
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);

        // 구르기 끝났는지?
        if (info.IsName(UnityConstant.Animator.Parameters.AC_Player.Bool.IsRoll) && info.normalizedTime >= 0.9f)
        {
            Player.StateMachine.ChangeState(input.move.IsNearlyZero() ? PlayerStateType.IDLE : PlayerStateType.MOVE);
            return;
        }
    }
}
