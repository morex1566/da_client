using UnityEngine;

public class PlayerJumpState : PlayerState
{
    private float jumpBlockTime = 0.2f;
    private float jumpBlockTimeElapsed = 0f;
    private float verticalVelocity = 0f;
    private const float GroundCheckRadius = 0.12f;
    private const float GroundCastUpOffset = 0.25f;
    private const float GroundCastExtraDistance = 0.25f;

    public PlayerJumpState(Player player) : base(player, PlayerStateType.Jump) {}

    public override void Enter()
    {
        jumpBlockTimeElapsed = 0f;
        verticalVelocity = Player.Data.JumpForce;
    }

    public override void Update(PlayerInputSnapshot inputSnapshot)
    {
        SetMoveDirection(inputSnapshot);
        SetLookDirection(inputSnapshot);

        Vector3 prevPosition = Player.transform.position;
        Move(inputSnapshot);

        ElapseBlockTime();
    }

    public override void Exit()
    {
        jumpBlockTimeElapsed = 0f;
        verticalVelocity = 0f;
    }

    protected override void Move(PlayerInputSnapshot inputSnapshot)
    {
        // XZ 이동
        Vector3 horizontalVelocity = Vector3.zero;
        if (!inputSnapshot.move.IsNearlyZero())
        {
            horizontalVelocity = new Vector3
            (
                Player.MoveDirection.x * Player.Data.MaxSpeed.x,
                0f,
                Player.MoveDirection.z * Player.Data.MaxSpeed.z
            );
        }

        // Y축 이동
        if (verticalVelocity > 0f)
        {
            // 올라가는 중 → 천천히 감속
            verticalVelocity += Player.Data.Gravity * Time.deltaTime;
        }
        else
        {
            // 내려가는 중 → 더 빠르게 낙하
            verticalVelocity += Player.Data.Gravity * Player.Data.FallMultiplier * Time.deltaTime;
        }

        // 최대 낙하 속도 제한
        verticalVelocity = Mathf.Max(verticalVelocity, Player.Data.MaxFallSpeed);

        Vector3 finalVelocity = horizontalVelocity;
        finalVelocity.y = verticalVelocity;
        Player.transform.position += finalVelocity * Time.deltaTime;
    }

    private void ElapseBlockTime()
    {
        jumpBlockTimeElapsed += Time.deltaTime;
    }

    public override void Evaluate(PlayerInputSnapshot inputSnapshot)
    {
        // 착지함
        if (jumpBlockTimeElapsed > jumpBlockTime && Physics.CheckSphere(Player.transform.position, GroundCheckRadius, UnityConstant.Layers.GroundMask))
        {
            Player.StateMachine.ChangeState(inputSnapshot.move.IsNearlyZero() ? PlayerStateType.IDLE : PlayerStateType.MOVE);
            return;
        }
    }
}
