using Net.Protocol;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using US2D.Network.Logic;

[DisallowMultipleComponent]
public partial class PlayerController : CreatureController, InputMappingContext.IPlayerActions
{
    [SerializeField] private SpringArm springArm = new();

    [SerializeField] private StateMachine<PlayerStateType> stateMachine;




    private InputSnapshot inputSnapshot = new InputSnapshot();





    public new PlayerData Data => base.Data as PlayerData;

    public SpringArm SpringArm => springArm;

    public StateMachine<PlayerStateType> StateMachine => stateMachine;

    public PlayerStateType CurrentStateType => stateMachine != null ? stateMachine.CurrentStateType : PlayerStateType.NONE;





    public event Action<PlayerController, InputSnapshot> OnColliderTriggered;

    public event Action<PlayerController, InputSnapshot> OnAttackTriggered;

    public event Action<PlayerController, InputSnapshot> OnReloadTriggered;

    public event Action<PlayerController, InputSnapshot> OnMoveTriggered;

    public event Action<PlayerController, InputSnapshot> OnRollTriggered;

    public event Action<PlayerController, InputSnapshot> OnLookTriggered;





    protected override void OnValidate()
    {
        base.OnValidate();

        Init();
        springArm.Update(LookDirection);
    }

    protected override void Init()
    {
        base.Init();

        var states = new Dictionary<PlayerStateType, IState<PlayerStateType>>
        {
            { PlayerStateType.IDLE, new PlayerIdleState(this) },
            { PlayerStateType.MOVE, new PlayerMoveState(this) },
            { PlayerStateType.ROLL, new PlayerRollState(this) },
            { PlayerStateType.DEAD, new PlayerDeadState(this) }
        };


        stateMachine = new StateMachine<PlayerStateType>(PlayerStateType.IDLE, states);
    }

    private void OnEnable()
    {
        InputManager.InputMappingContext.Player.Move.performed += OnMove;
        InputManager.InputMappingContext.Player.Move.canceled += OnMove;

        InputManager.InputMappingContext.Player.Look.performed += OnLook;
        InputManager.InputMappingContext.Player.Look.canceled += OnLook;

        InputManager.InputMappingContext.Player.Attack.performed += OnAttack;

        InputManager.InputMappingContext.Player.Roll.performed += OnRoll;

        InputManager.InputMappingContext.Player.Reload.performed += OnReload;
    }

    private void OnDisable()
    {
        InputManager.InputMappingContext.Player.Move.performed -= OnMove;
        InputManager.InputMappingContext.Player.Move.canceled -= OnMove;

        InputManager.InputMappingContext.Player.Look.performed -= OnLook;
        InputManager.InputMappingContext.Player.Look.canceled -= OnLook;

        InputManager.InputMappingContext.Player.Attack.performed -= OnAttack;

        InputManager.InputMappingContext.Player.Roll.performed -= OnRoll;

        InputManager.InputMappingContext.Player.Reload.performed -= OnReload;
    }

    private void Update()
    {
        if (stateMachine == null)
        {
            return; // Init 이전 프레임이면 상태 업데이트 불가
        }

        InputSnapshot input = inputSnapshot.Consume();

        stateMachine.Update(input);
        springArm.Update(LookDirection);
    }

    private void OnDrawGizmos()
    {
        if (springArm == null || springArm.PivotTransform == null || springArm.SocketTransform == null)
        {
            return; // 씬에서 SpringArm Transform이 연결되지 않았으면 보조선 생략
        }

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + MoveDirection);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(springArm.PivotTransform.position, springArm.SocketTransform.position);
    }
}

public partial class PlayerController
{
    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        inputSnapshot.move = input;

        OnMoveTriggered?.Invoke(this, inputSnapshot);
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        inputSnapshot.look = input;

        OnLookTriggered?.Invoke(this, inputSnapshot);
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        inputSnapshot.attackPressed = true;

        OnAttackTriggered?.Invoke(this, inputSnapshot);
    }

    public void OnRoll(InputAction.CallbackContext context)
    {
        inputSnapshot.rollPressed = true;

        OnRollTriggered?.Invoke(this, inputSnapshot);
    }

    public void OnReload(InputAction.CallbackContext context)
    {
        inputSnapshot.reloadPressed = true;

        OnReloadTriggered?.Invoke(this, inputSnapshot);
    }

    public void OnInspect(InputAction.CallbackContext context) {}

    public void OnInteract(InputAction.CallbackContext context) {}

    public void OnTriggerEnter(Collider other)
    {
        OnColliderTriggered?.Invoke(this, inputSnapshot);
    }
}

public partial class PlayerController
{
    [NetSendMessage(PacketTypeId.Transform)]
    public void OnSendMove(Net.Protocol.Transform payload) {}
}
