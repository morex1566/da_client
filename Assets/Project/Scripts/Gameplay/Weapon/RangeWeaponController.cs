using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[DisallowMultipleComponent]
public partial class RangeWeaponController : WeaponController
{
    [SerializeField] private WeaponAttachment grip = null;

    [SerializeField] private Transform root = null;

    [SerializeField] private Transform weaponSlot = null;

    [SerializeField] private PlayerController playerController = null;

    [SerializeField] private List<WeaponAttachment> attachments = new();

    private ObjectPool<Projectile> projectilePool;

    public IReadOnlyList<WeaponAttachment> Attachments => attachments;

    public WeaponAttachment Grip => grip;

    public Transform Root => root;

    public float Recoil { get; set; }

    public float Accurancy { get; set; }

    public float Weight { get; set; }

    public float RPM { get; set; }

    public float Damage { get; set; }

    public int MaxAmmo { get; set; }

    public int CurrAmmo { get; set; }

    public Vector3 LookDirection { get; set; } = Vector3.right;

    public RangeWeaponStateType StateType { get; set; } = RangeWeaponStateType.Idle;

    public event Action<RangeWeaponController> OnAttackTriggered;

    public event Action<RangeWeaponController> OnReloadTriggered;

    protected override void Init()
    {
        base.Init();

        playerController ??= FindFirstObjectByType<PlayerController>();
        BuildSlots();
        CreateProjectilePool();
    }

    private void OnEnable()
    {
        Init();
        if (playerController == null)
        {
            return; // 플레이어 컨트롤러가 없으면 입력 이벤트 연결 불가
        }

        playerController.OnLookTriggered += OnLook;
        playerController.OnAttackTriggered += OnAttack;
        playerController.OnReloadTriggered += OnReload;
    }

    private void OnDisable()
    {
        if (playerController == null)
        {
            return; // 연결된 플레이어가 없으면 해제할 이벤트도 없음
        }

        playerController.OnLookTriggered -= OnLook;
        playerController.OnAttackTriggered -= OnAttack;
        playerController.OnReloadTriggered -= OnReload;
    }

    private void OnDrawGizmos()
    {
        if (weaponSlot == null)
        {
            return; // 발사 슬롯이 없으면 조준 보조선을 그릴 기준이 없음
        }

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + LookDirection);
    }

    private void LateUpdate()
    {
        UpdateAimTransform();
    }

    public void BuildSlots()
    {
#if UNITY_EDITOR
        if (Application.isPlaying == false)
        {
            UnityEditor.EditorApplication.delayCall += () =>
            {
                if (this == null)
                {
                    return; // 에디터 지연 호출 사이에 오브젝트가 삭제된 경우 방어
                }

                BuildSlotsImmediate();
            };

            return;
        }
#endif

        BuildSlotsImmediate();
    }

    public T FindWeaponAttachment<T>() where T : WeaponAttachment
    {
        return WeaponAttachmentTreeBuilder.FindWeaponAttachment<T>(attachments);
    }

    public WeaponAttachment FindWeaponAttachmentByType(WeaponAttachmentType type)
    {
        for (int i = 0; i < attachments.Count; i++)
        {
            WeaponAttachment result = FindWeaponAttachmentByType(attachments[i], type);
            if (result != null)
            {
                return result; // 첫 번째로 발견한 타입 매칭 파츠 반환
            }
        }

        return null;
    }

    public TValue FindWeaponAttachmentDataValue<TValue>(WeaponAttachmentType type, string propertyName) where TValue : class
    {
        WeaponAttachment attachment = FindWeaponAttachmentByType(type);
        if (attachment == null || attachment.Data == null)
        {
            return null; // 요청한 파츠 또는 데이터가 없으면 값 조회 불가
        }

        var property = attachment.Data.GetType().GetProperty(propertyName);
        return property?.GetValue(attachment.Data) as TValue;
    }

    private void BuildSlotsImmediate()
    {
        if (grip == null)
        {
            return; // 루트 그립 프리팹이 없으면 부착 트리를 만들 수 없음
        }

        WeaponAttachmentTreeBuilder.BuildSlots(root, attachments, grip);
    }

    private void CreateProjectilePool()
    {
        projectilePool = new ObjectPool<Projectile>
        (
            OnCreateProjectile,
            OnGetProjectile,
            OnReleaseProjectile,
            OnDestroyProjectile,
            false,
            MaxAmmo,
            Mathf.Max(MaxAmmo, 1)
        );
    }

    private Projectile OnCreateProjectile()
    {
        var projectile = FindWeaponAttachmentDataValue<Projectile>(WeaponAttachmentType.Assembly, "Projectile");
        if (projectile == null)
        {
            return null; // Assembly 파츠에 Projectile이 없으면 풀 생성을 건너뜀
        }

        Projectile projectileInst = Instantiate(projectile, transform.position, transform.rotation, transform);
        projectileInst.gameObject.SetActive(false);

        return projectileInst;
    }

    private void OnGetProjectile(Projectile projectile)
    {
        if (projectile == null)
        {
            return; // 풀에서 null이 넘어오면 활성화할 대상이 없음
        }

        Transform spawnTransform = weaponSlot != null ? weaponSlot : transform;
        projectile.transform.SetParent(transform, false);
        projectile.transform.SetPositionAndRotation(spawnTransform.position, spawnTransform.rotation);
        projectile.gameObject.SetActive(true);
    }

    private void OnReleaseProjectile(Projectile projectile)
    {
        if (projectile == null)
        {
            return; // 반환 대상이 없으면 풀 복귀 처리 생략
        }

        projectile.gameObject.SetActive(false);
        projectile.transform.SetParent(transform, false);
    }

    private void OnDestroyProjectile(Projectile projectile)
    {
        if (projectile == null)
        {
            return; // 파괴 대상이 없으면 종료
        }

        Destroy(projectile.gameObject);
    }

    private bool TryFire()
    {
        if (CurrAmmo <= 0)
        {
            return false; // 탄약이 없으면 발사 실패
        }

        CurrAmmo--;
        return true;
    }

    private bool TryReload()
    {
        if (CurrAmmo >= MaxAmmo)
        {
            return false; // 이미 최대 탄약이면 장전 불필요
        }

        CurrAmmo = MaxAmmo;
        return true;
    }

    private WeaponAttachment FindWeaponAttachmentByType(WeaponAttachment attachment, WeaponAttachmentType type)
    {
        if (attachment == null)
        {
            return null; // 빈 노드는 탐색 대상이 아님
        }

        if (attachment.Data != null && attachment.Data.Type == type)
        {
            return attachment; // 현재 파츠가 요청 타입이면 즉시 반환
        }

        Transform attachmentTransform = attachment.transform;
        for (int i = 0; i < attachmentTransform.childCount; i++)
        {
            WeaponAttachment child = attachmentTransform.GetChild(i).GetComponent<WeaponAttachment>();
            WeaponAttachment result = FindWeaponAttachmentByType(child, type);
            if (result != null)
            {
                return result; // 하위 트리에서 찾은 파츠 반환
            }
        }

        return null;
    }
}

public partial class RangeWeaponController
{
    public void OnLook(PlayerController playerController, InputSnapshot input)
    {
        UpdateAimTransform();
    }

    private void UpdateAimTransform()
    {
        if (playerController == null)
        {
            return; // 조준 기준 플레이어가 없으면 무기 방향 계산 불가
        }

        if (playerController.LookDirection.sqrMagnitude < 0.0001f)
        {
            return; // 유효한 조준 방향이 없으면 마지막 무기 방향 유지
        }

        LookDirection = playerController.LookDirection.normalized;

        SpringArm springArm = playerController.SpringArm;
        if (springArm != null && springArm.SocketTransform != null)
        {
            transform.SetPositionAndRotation(springArm.SocketTransform.position, springArm.SocketTransform.rotation);
            return; // SpringArm 소켓이 있으면 무기를 소켓 위치/회전에 고정
        }

        transform.right = LookDirection;
    }

    public void OnAttack(PlayerController playerController, InputSnapshot input)
    {
        if (!TryFire())
        {
            return; // 발사 조건이 맞지 않으면 투사체를 꺼내지 않음
        }

        projectilePool?.Get();
        OnAttackTriggered?.Invoke(this);
    }

    public void OnReload(PlayerController playerController, InputSnapshot input)
    {
        if (!TryReload())
        {
            return; // 장전이 필요 없는 상태면 이벤트를 발생시키지 않음
        }

        OnReloadTriggered?.Invoke(this);
    }
}
