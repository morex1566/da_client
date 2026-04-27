using UnityEngine;

[RequireComponent(typeof(RangeWeaponController))]
[DisallowMultipleComponent]
public class RangeWeaponView : MonoBehaviour
{
    [SerializeField, ReadOnly] private RangeWeaponController weaponController = null;

    [SerializeField] private Animator animator = null;

    [SerializeField] private SpriteRenderer spriter = null;

    [SerializeField] private AudioSource audioSource = null;

    [SerializeField] private float lookDeadZone = 0.1f;

    private void OnValidate()
    {
        Init();
    }

    private void Awake()
    {
        Init();
    }

    private void OnEnable()
    {
        Init();
        if (weaponController == null)
        {
            return; // 무기 컨트롤러가 없으면 발사 이벤트 연결 불가
        }

        weaponController.OnAttackTriggered += SetOnFireTrigger;
    }

    private void OnDisable()
    {
        if (weaponController == null)
        {
            return; // 연결된 무기 컨트롤러가 없으면 해제할 이벤트도 없음
        }

        weaponController.OnAttackTriggered -= SetOnFireTrigger;
    }

    public void Init()
    {
        weaponController = GetComponent<RangeWeaponController>();
        animator = Utls.FindComponent<Animator>(gameObject);
        spriter = Utls.FindComponent<SpriteRenderer>(gameObject);
        audioSource = Utls.FindComponent<AudioSource>(gameObject);
    }

    private void Update()
    {
        UpdateFlip();
        UpdateAnimationParameters();
    }

    public void UpdateFlip()
    {
        if (weaponController.LookDirection.x < lookDeadZone * -1f)
        {
            spriter.flipY = true; // 왼쪽 조준이면 무기 스프라이트를 뒤집음
            return;
        }

        if (weaponController.LookDirection.x > lookDeadZone)
        {
            spriter.flipY = false; // 오른쪽 조준이면 기본 방향으로 복귀
        }
    }

    public void UpdateAnimationParameters()
    {
        if (weaponController == null || animator == null)
        {
            return; // 상태 또는 Animator 참조가 없으면 애니메이션 동기화 불가
        }

        animator.SetBool(UnityConstant.Animator.Parameters.AC_RangeWeapon.Bool.IsReload, weaponController.StateType == RangeWeaponStateType.Reload);
    }

    public void SetOnFireTrigger(RangeWeaponController weaponController)
    {
        if (animator == null)
        {
            return; // Animator가 없으면 발사 트리거 반영 불가
        }

        animator.SetTrigger(UnityConstant.Animator.Parameters.AC_RangeWeapon.Trigger.OnFire);
    }

    public void AudioPlayFire()
    {
        if (weaponController == null || audioSource == null)
        {
            return; // 무기 데이터 또는 AudioSource가 없으면 사운드 재생 불가
        }

        var audio = weaponController.FindWeaponAttachmentDataValue<AudioClip>(WeaponAttachmentType.Muzzle, "FireSfxOverride");
        if (audio == null)
        {
            return; // Muzzle 파츠에 발사 사운드가 없으면 재생 생략
        }

        audioSource.PlayOneShot(audio);
    }
}
