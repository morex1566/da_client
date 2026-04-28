using UnityEngine;

[RequireComponent(typeof(PlayerController))]
[DisallowMultipleComponent]
public class PlayerView : MonoBehaviour
{
    [SerializeField, ReadOnly] private PlayerController playerController = null;

    [field : SerializeField] public Animator Animator { get; private set; } = null;

    [field : SerializeField] public SpriteRenderer Spriter { get; private set; } = null;

    [SerializeField] private float lookDeadZone = 0.1f;



    public float LookDeadZone => lookDeadZone;



    private void OnValidate()
    {
        Init();
    }

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        playerController = GetComponent<PlayerController>();
        Animator = Utls.FindComponent<Animator>(gameObject);
        Spriter = Utls.FindComponent<SpriteRenderer>(gameObject);
    }

    private void Update()
    {
        UpdateFlip();
        UpdateAnimationParameters();
    }

    public void UpdateFlip()
    {
        if (Mathf.Abs(playerController.CurrLookDirection.x) < lookDeadZone)
        {
            return;
        }

        Spriter.flipX = playerController.CurrLookDirection.x < 0f;
    }

    public void UpdateAnimationParameters()
    {
        if (playerController == null || Animator == null)
        {
            return;
        }

        PlayerStateType stateType = playerController.CurrentStateType;

        Animator.SetBool(UnityConstant.Animator.Parameters.AC_Player.Bool.IsIdle, stateType == PlayerStateType.IDLE);
        Animator.SetBool(UnityConstant.Animator.Parameters.AC_Player.Bool.IsMoving, stateType == PlayerStateType.MOVE);
        Animator.SetBool(UnityConstant.Animator.Parameters.AC_Player.Bool.IsRoll, stateType == PlayerStateType.ROLL);
        Animator.SetBool(UnityConstant.Animator.Parameters.AC_Player.Bool.IsJump, stateType == PlayerStateType.JUMP);
    }
}
