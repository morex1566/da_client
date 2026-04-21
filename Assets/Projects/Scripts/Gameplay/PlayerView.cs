using UnityEngine;

[RequireComponent(typeof(Player))]
[DisallowMultipleComponent]
public class PlayerView : MonoBehaviour
{
    [SerializeField, ReadOnly] private Player player = null;

    [SerializeField] private Animator animator = null;

    [SerializeField] private SpriteRenderer spriter = null;

    [SerializeField] private float lookDeadZone = 0.1f;





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
        player = GetComponent<Player>();
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
        spriter.flipX = player.LookDirection.x < lookDeadZone * -1f ? true : player.LookDirection.x > lookDeadZone ? false : spriter.flipX;
    }

    public void UpdateAnimationParameters()
    {
        animator.SetBool(UnityConstant.Animator.Parameters.AC_Player.Bool.IsMoving, player.IsMoving);
        animator.SetBool(UnityConstant.Animator.Parameters.AC_Player.Bool.IsGroggy, player.IsGroggy);
        animator.SetBool(UnityConstant.Animator.Parameters.AC_Player.Bool.IsRoll, player.IsRolling);
    }
}
