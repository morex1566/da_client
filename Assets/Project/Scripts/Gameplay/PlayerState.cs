using UnityEngine;

public enum PlayerStateType
{
    NONE = 0,
    IDLE = 1,
    MOVE = 2,
    ROLL = 3,
    DEAD = 4,
    Jump = 5
}


public abstract class PlayerState
{
    protected Player Player { get; private set; }

    public PlayerStateType StateType { get; private set; } = PlayerStateType.NONE;

    protected PlayerState(Player player, PlayerStateType stateType)
    {
        Player = player;
        StateType = stateType;
    }

    public virtual void Enter() {}

    public virtual void Exit() {}

    // 입력을 처리해서 실제로 Player 데이터에 반영
    public virtual void Update(PlayerInputSnapshot inputSnapshot) {}

    public virtual bool CanTransitTo(PlayerStateType nextStateType)
    {
        return true;
    }

    // Player 데이터나 입력을 통해 다른 상태로 넘어가야하는지 검사
    public virtual void Evaluate(PlayerInputSnapshot inputSnapshot) { }

    protected virtual void SetMoveDirection(PlayerInputSnapshot inputSnapshot)
    {
        if (inputSnapshot.move.IsNearlyZero()) return;

        Vector3 input = inputSnapshot.move.normalized;
        Player.MoveDirection = new Vector3(input.x, 0, input.y);
    }

    protected virtual void SetLookDirection(PlayerInputSnapshot inputSnapshot)
    {
        if (inputSnapshot.look.IsNearlyZero()) return;

        Player.LookDirection = (Utls.GetMouseWorldPosition() - Player.transform.position).normalized;
    }

    protected virtual void Move(PlayerInputSnapshot inputSnapshot)
    {
        if (inputSnapshot.move.IsNearlyZero()) return;

        Vector3 frameVelocity = new Vector3
        (
            Player.MoveDirection.x * Player.Data.MaxSpeed.x,
            0,
            Player.MoveDirection.z * Player.Data.MaxSpeed.z
        );

        Player.transform.position += frameVelocity * Time.deltaTime;
    }

    protected virtual void Rotate()
    {

    }
}
