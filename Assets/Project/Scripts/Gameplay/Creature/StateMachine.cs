using System;
using System.Collections.Generic;
using UnityEngine;

public interface IState<TStateType> where TStateType : struct, Enum
{
    TStateType StateType { get; }

    void Enter();

    void Exit();

    void Update(InputSnapshot inputSnapshot);

    TStateType? Evaluate(InputSnapshot inputSnapshot);

    bool CanTransitTo(TStateType nextStateType);
}

[Serializable]
public class StateMachine<TStateType> where TStateType : struct, Enum
{
    private readonly Dictionary<TStateType, IState<TStateType>> states = new();

    public IState<TStateType> CurrentState { get; private set; } = null;

    public TStateType CurrentStateType => CurrentState != null ? CurrentState.StateType : default;

    public StateMachine(TStateType initialStateType, IReadOnlyDictionary<TStateType, IState<TStateType>> sourceStates)
    {
        foreach (var state in sourceStates)
        {
            states.Add(state.Key, state.Value);
        }

        if (states.TryGetValue(initialStateType, out IState<TStateType> initialState) == false)
        {
            Debug.LogError($"[StateMachine] Initial state is not registered: {initialStateType}");
            return; // 시작 상태가 없으면 상태 머신을 안전하게 비활성 상태로 둠
        }

        CurrentState = initialState;
        CurrentState.Enter();
    }

    public bool ChangeState(TStateType nextType)
    {
        if (EqualityComparer<TStateType>.Default.Equals(CurrentState.StateType, nextType))
        {
            return false; // 같은 상태로 다시 진입하지 않음
        }

        if (CurrentState.CanTransitTo(nextType) == false)
        {
            return false; // 현재 상태가 막는 전이면 상태 유지
        }

        if (states.TryGetValue(nextType, out IState<TStateType> nextState) == false)
        {
            Debug.LogError($"[StateMachine] State is not registered: {nextType}");
            return false; // 등록되지 않은 상태 요청은 무시
        }

        CurrentState.Exit();
        CurrentState = nextState;
        CurrentState.Enter();

        return true;
    }

    public void Update(InputSnapshot inputSnapshot)
    {
        CurrentState.Update(inputSnapshot);

        TStateType? nextType = CurrentState.Evaluate(inputSnapshot);
        if (nextType.HasValue == false)
        {
            return; // 상태가 유지되는 프레임이면 전이 처리 생략
        }

        ChangeState(nextType.Value);
    }
}
