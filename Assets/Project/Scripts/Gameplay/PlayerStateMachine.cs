using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine
{
    private readonly Dictionary<PlayerStateType, PlayerState> states = new();

    public PlayerState CurrentState { get; private set; } = null;




    public PlayerStateMachine(Player player)
    {
        Init(player);
    }

    private void Init(Player player)
    {
        states.Add(PlayerStateType.IDLE, new PlayerIdleState(player));
        states.Add(PlayerStateType.MOVE, new PlayerMoveState(player));
        states.Add(PlayerStateType.ROLL, new PlayerRollState(player));
        states.Add(PlayerStateType.DEAD, new PlayerDeadState(player));
        states.Add(PlayerStateType.Jump, new PlayerJumpState(player));

        CurrentState = states[PlayerStateType.IDLE];
        CurrentState.Enter();
    }

    public void ChangeState(PlayerStateType nextType)
    {
        if (CurrentState.StateType == nextType || CurrentState.CanTransitTo(nextType) == false) return;

        CurrentState.Exit();
        CurrentState = states[nextType];
        CurrentState.Enter();
    }

    public void Update(PlayerInputSnapshot inputSnapshot)
    {
        CurrentState.Update(inputSnapshot);
        CurrentState.Evaluate(inputSnapshot);
    }
}
