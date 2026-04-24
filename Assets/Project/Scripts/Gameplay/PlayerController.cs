using Net.Protocol;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using US2D.Network.Logic;



public struct PlayerInputSnapshot
{
    // Delta
    public Vector2 move;
    public Vector2 look;

    // Trigger
    public bool attackPressed;
    public bool rollPressed;
    public bool reloadPressed;
    public bool jumpPressed;

    public bool IsEmpty => Equals(default(PlayerInputSnapshot));

    public PlayerInputSnapshot Consume()
    {
        PlayerInputSnapshot value = this;
        this = default;
        move = value.move;
        look = value.look;

        return value;
    }
}



[RequireComponent(typeof(Player))]
[DisallowMultipleComponent]
public partial class PlayerController : MonoBehaviour, InputMappingContext.IPlayerActions
{
    [SerializeField, ReadOnly] private Player player = null;

    private PlayerInputSnapshot inputSnapshot = new PlayerInputSnapshot();




    /// <summary>
    /// float1 : 현재 체력, float2: 최대 체력
    /// </summary>
    public Action<float, float> OnColliderTriggered;

    public event Action OnAttackTriggered;

    public event Action OnReloadTriggered;

    public event Action OnMoveTriggered;

    public event Action OnRollTriggered;

    public event Action OnJumpTriggered;

    public event Action<UnityEngine.Transform, Vector2> OnLookTriggered;

    private void OnValidate()
    {
        Init();
    }

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        player = GetComponent<Player>();
    }

    private void OnEnable()
    {
        InputManager.InputMappingContext.Player.Move.performed += OnMove;
        InputManager.InputMappingContext.Player.Move.canceled += OnMove;
        InputManager.InputMappingContext.Player.Look.performed += OnLook;
        InputManager.InputMappingContext.Player.Look.canceled += OnLook;
        InputManager.InputMappingContext.Player.Attack.performed += OnAttack;
        InputManager.InputMappingContext.Player.Roll.performed += OnRoll;
        InputManager.InputMappingContext.Player.Jump.performed += OnJump;
    }

    private void OnDisable()
    {
        InputManager.InputMappingContext.Player.Move.performed -= OnMove;
        InputManager.InputMappingContext.Player.Move.canceled -= OnMove;
        InputManager.InputMappingContext.Player.Look.performed -= OnLook;
        InputManager.InputMappingContext.Player.Look.canceled -= OnLook;
        InputManager.InputMappingContext.Player.Attack.performed -= OnAttack;
        InputManager.InputMappingContext.Player.Roll.performed -= OnRoll;
        InputManager.InputMappingContext.Player.Jump.performed -= OnJump;
    }

    private void Update()
    {
        player.StateMachine.Update(inputSnapshot.Consume());
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        // MoveDirection
        Gizmos.DrawLine(player.transform.position, player.transform.position + player.MoveDirection);

        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(player.transform.position, 0.1f);
    }
}

/// <summary>
/// 입력, 트리거 처리
/// </summary>
public partial class PlayerController
{
    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        inputSnapshot.move = input;

        OnMoveTriggered?.Invoke();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        inputSnapshot.look = input;

        OnLookTriggered?.Invoke(transform, player.LookDirection);
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        inputSnapshot.attackPressed = true;

        OnAttackTriggered?.Invoke();
    }

    public void OnRoll(InputAction.CallbackContext context)
    {
        inputSnapshot.rollPressed = true;

        OnRollTriggered?.Invoke();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        inputSnapshot.jumpPressed = true;

        OnJumpTriggered?.Invoke();
    }

    public void OnReload(InputAction.CallbackContext context)
    {
        inputSnapshot.reloadPressed = true;

        OnReloadTriggered?.Invoke();
    }

    public void OnInspect(InputAction.CallbackContext context) {}

    public void OnInteract(InputAction.CallbackContext context) {}

    public void OnTriggerEnter(Collider other)
    {
        OnColliderTriggered?.Invoke(player.CurrentHp, player.Data.MaxHp);
    }
}

/// <summary>
/// 네트워크 메서드
/// </summary>
public partial class PlayerController
{
    [NetSendMessage(PacketTypeId.Transform)]
    public void OnSendMove(Net.Protocol.Transform payload) {}
}
