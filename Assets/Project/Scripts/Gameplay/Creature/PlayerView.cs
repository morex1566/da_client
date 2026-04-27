using UnityEngine;

[RequireComponent(typeof(PlayerController))]
[DisallowMultipleComponent]
public class PlayerView : MonoBehaviour
{
    [SerializeField, ReadOnly] private PlayerController playerController = null;

    [SerializeField] private Animator animator = null;

    [SerializeField] private SpriteRenderer spriter = null;

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
        animator = Utls.FindComponent<Animator>(gameObject);
        spriter = Utls.FindComponent<SpriteRenderer>(gameObject);
    }

    private void Update()
    {
        UpdateFlip();
        UpdateAnimationParameters();
    }

    public void UpdateFlip()
    {
        if (Mathf.Abs(playerController.LookDirection.x) < lookDeadZone)
        {
            return;
        }

        spriter.flipX = playerController.LookDirection.x < 0f;
    }

    public void UpdateAnimationParameters()
    {
        if (playerController == null || animator == null)
        {
            return;
        }

        PlayerStateType stateType = playerController.CurrentStateType;

        animator.SetBool(UnityConstant.Animator.Parameters.AC_Player.Bool.IsIdle, stateType == PlayerStateType.IDLE);
        animator.SetBool(UnityConstant.Animator.Parameters.AC_Player.Bool.IsMoving, stateType == PlayerStateType.MOVE);
        animator.SetBool(UnityConstant.Animator.Parameters.AC_Player.Bool.IsRoll, stateType == PlayerStateType.ROLL);
        animator.SetBool(UnityConstant.Animator.Parameters.AC_Player.Bool.IsJump, stateType == PlayerStateType.JUMP);
    }
}
