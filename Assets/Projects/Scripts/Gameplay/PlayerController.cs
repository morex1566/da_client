using Net.Protocol;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using US2D.Network.Logic;

[RequireComponent(typeof(Player))]
[DisallowMultipleComponent]
public partial class PlayerController : MonoBehaviour, InputMappingContext.IPlayerActions
{
    [SerializeField, ReadOnly] private Player player = null;




    /// <summary>
    /// float1 : 현재 체력, float2: 최대 체력
    /// </summary>
    public Action<float, float> OnColliderTriggered;

    public event Action OnFireTriggered;

    public event Action OnReloadTriggered;

    public event Action OnMoveTriggered;

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
        InputManager.InputMappingContext.Player.Fire.performed += OnFire;
        InputManager.InputMappingContext.Player.Look.performed += OnLook;
        InputManager.InputMappingContext.Player.Reload.performed += OnReload;
    }

    private void OnDisable()
    {
        InputManager.InputMappingContext.Player.Move.performed -= OnMove;
        InputManager.InputMappingContext.Player.Move.canceled -= OnMove;
        InputManager.InputMappingContext.Player.Fire.performed -= OnFire;
        InputManager.InputMappingContext.Player.Look.performed -= OnLook;
        InputManager.InputMappingContext.Player.Reload.performed -= OnReload;
    }

    private void Update()
    {
        UpdateMovement();
    }

    private void UpdateMovement()
    {
        if (player == null)
        {
            return;
        }

        Vector2 frameVelocity = new Vector2
        (
            player.MoveDirection.x * player.Data.MaxSpeed.x,
            player.MoveDirection.y * player.Data.MaxSpeed.y
        );

        transform.position += (Vector3)frameVelocity * Time.deltaTime;
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

        player.MoveDirection = input.IsNearlyZero() ? Vector2.zero : input.normalized;
        player.IsMoving = player.MoveDirection.IsNotNearlyZero();
        OnMoveTriggered?.Invoke();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        if (input.IsNearlyZero())
        {
            return;
        }

        player.LookDirection = (Utls.GetMouseWorldPosition() - transform.position).normalized;

        OnLookTriggered?.Invoke(transform, player.LookDirection);
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }

        OnFireTriggered?.Invoke();
    }


    public void OnInspect(InputAction.CallbackContext context) { }

    public void OnInteract(InputAction.CallbackContext context) { }
    public void OnCrouch(InputAction.CallbackContext context) { }
    public void OnJump(InputAction.CallbackContext context) { }
    public void OnPrevious(InputAction.CallbackContext context) { }
    public void OnNext(InputAction.CallbackContext context) { }
    public void OnSprint(InputAction.CallbackContext context) { }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        OnColliderTriggered?.Invoke(player.CurrentHp, player.Data.MaxHp);
    }

    public void OnReload(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }

        OnReloadTriggered?.Invoke();
    }
}

/// <summary>
/// 네트워크 메서드
/// </summary>
public partial class PlayerController
{
    [NetSendMessage(PacketTypeId.Transform)]
    public void OnSendMove(Net.Protocol.Transform payload) { }
}
