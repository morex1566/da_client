using UnityEngine;

[DisallowMultipleComponent]
public class RangeWeaponView : MonoBehaviour
{
    [SerializeField] private RangeWeapon weapon = null;

    [SerializeField] private Animator animator = null;

    [SerializeField] private SpriteRenderer spriter = null;

    [SerializeField] private float lookDeadZone = 0.1f;


    [Header("Internal")]
    [SerializeField] private PlayerController playerController = null;




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
        weapon = Utls.FindComponent<RangeWeapon>(gameObject);
        animator = Utls.FindComponent<Animator>(gameObject);
        spriter = Utls.FindComponent<SpriteRenderer>(gameObject);
        playerController = Utls.FindComponentByTag<PlayerController>(UnityConstant.Tags.Player);
    }

    private void OnEnable()
    {
        playerController.OnFireTriggered += SetOnFireTrigger;
    }

    private void OnDisable()
    {
        playerController.OnFireTriggered -= SetOnFireTrigger;
    }

    private void Update()
    {
        UpdateFlip();
        UpdateAnimationParameters();
    }

    // 마우스 입력 시 콜, 크로스헤어가 플레이어 기준 좌/우 위치에 따라 플립
    public void UpdateFlip()
    {
        spriter.flipY = weapon.LookDirection.x < lookDeadZone * -1f ? true : weapon.LookDirection.x > lookDeadZone ? false : spriter.flipY;
    }

    public void UpdateAnimationParameters()
    {
        animator.SetBool(UnityConstant.Animator.Parameters.AC_Weapon_Pistol.Bool.IsReload, weapon.State == RangeWeaponState.Reload);
    }

    public void SetOnFireTrigger()
    {
        animator.SetTrigger(UnityConstant.Animator.Parameters.AC_Weapon_Pistol.Trigger.OnFire);
    }
}
