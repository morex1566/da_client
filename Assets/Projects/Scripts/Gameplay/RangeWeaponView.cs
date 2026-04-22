using UnityEngine;

[RequireComponent(typeof(RangeWeapon))]
[DisallowMultipleComponent]
public class RangeWeaponView : MonoBehaviour
{
    [SerializeField, ReadOnly] private RangeWeapon weapon = null;

    [SerializeField] private Animator animator = null;

    [SerializeField] private SpriteRenderer spriter = null;

    [SerializeField] private AudioSource audioSource = null;

    [SerializeField] private float lookDeadZone = 0.1f;




    [Header("Internal")]
    [SerializeField] private RangeWeaponController weaponController = null;





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
        weapon = GetComponent<RangeWeapon>();
        weaponController = GetComponent<RangeWeaponController>();
        animator = Utls.FindComponent<Animator>(gameObject);
        spriter = Utls.FindComponent<SpriteRenderer>(gameObject);
        audioSource = Utls.FindComponent<AudioSource>(gameObject);
    }

    private void OnEnable()
    {
        weaponController.OnHandleFireTriggered += SetOnFireTrigger;
    }

    private void OnDisable()
    {
        weaponController.OnHandleFireTriggered -= SetOnFireTrigger;
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
        animator.SetBool(UnityConstant.Animator.Parameters.AC_Weapon_Pistol.Bool.IsReload,  weapon.StateType == RangeWeaponStateType.Reload);
    }

    public void SetOnFireTrigger(int currentAmmo, int maxAmmo)
    {
        animator.SetTrigger(UnityConstant.Animator.Parameters.AC_Weapon_Pistol.Trigger.OnFire);
    }

    public void AudioPlayFire()
    {
        audioSource.PlayOneShot(weapon.Data.Fire);
    }
}
