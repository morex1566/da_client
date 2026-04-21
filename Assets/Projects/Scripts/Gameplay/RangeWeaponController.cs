using System;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.TextCore.Text;

[RequireComponent(typeof(RangeWeapon))]
[DisallowMultipleComponent]
public partial class RangeWeaponController : MonoBehaviour
{
    [SerializeField, ReadOnly] private RangeWeapon weapon;

    [SerializeField] private Transform weaponSocket;

    [SerializeField] private Transform weaponRotationPivot;

    private ObjectPool<Projectile> projectilePool;

    [SerializeField] private float lookDeadZone = 0.1f;

    [Header("External")]
    [SerializeField] private PlayerController playerController;




    public event Action<Vector2> OnHandleLookDirectionChanged;

    public event Action<int, int> OnHandleFireTriggered;

    public event Action<int, int> OnHandleReloadTriggered;





    private void Awake()
    {
        Init();
    }

    private void OnValidate()
    {
        Init();
    }

    private void Init()
    {
        weapon = GetComponent<RangeWeapon>();
        playerController = Utls.FindComponentByTag<PlayerController>(UnityConstant.Tags.Player);

        projectilePool = new ObjectPool<Projectile>
        (
            OnCreateProjectile,
            OnGetProjectile,
            OnReleaseProjectile,
            OnDestroyProjectile,
            false,
            weapon.MaxAmmo,
            Mathf.Max(weapon.MaxAmmo, 1)
        );
    }

    private void OnEnable()
    {
        playerController.OnLookTriggered += HandleLookTriggered;
        playerController.OnFireTriggered += HandleFireTriggered;
        playerController.OnReloadTriggered += HandleReloadTriggered;
    }

    private void OnDisable()
    {
        playerController.OnLookTriggered -= HandleLookTriggered;
        playerController.OnFireTriggered -= HandleFireTriggered;
        playerController.OnReloadTriggered -= HandleReloadTriggered;
    }

    private void OnDrawGizmos()
    {
        if (weaponRotationPivot == null || weaponSocket == null)
        {
            return;
        }

        Gizmos.color = Color.red;
        Gizmos.DrawLine(weaponRotationPivot.position, weaponSocket.position);
    }

    private Projectile OnCreateProjectile()
    {
        GameObject projectileInst = Instantiate(weapon.Data.projectilePf, weapon.transform.position, weapon.transform.rotation, weapon.transform);
        projectileInst.gameObject.SetActive(false);
        return projectileInst.GetComponent<Projectile>();
    }

    private void OnGetProjectile(Projectile projectile)
    {
        if (projectile == null)
        {
            return;
        }

        Transform spawnTransform = weaponSocket != null ? weaponSocket : transform;
        projectile.transform.SetParent(transform, false);
        projectile.transform.SetPositionAndRotation(spawnTransform.position, spawnTransform.rotation);
        projectile.gameObject.SetActive(true);
    }

    private void OnReleaseProjectile(Projectile projectile)
    {
        if (projectile == null)
        {
            return;
        }

        projectile.gameObject.SetActive(false);
        projectile.transform.SetParent(transform, false);
    }

    private void OnDestroyProjectile(Projectile projectile)
    {
        if (projectile == null)
        {
            return;
        }

        Destroy(projectile.gameObject);
    }

    private bool TryFire()
    {
        if (weapon.CurrAmmo <= 0) return false;

        weapon.CurrAmmo--;
        return true;
    }

    private bool TryReload()
    {
        if (weapon.CurrAmmo >= weapon.MaxAmmo) return false;

        weapon.CurrAmmo = weapon.MaxAmmo;
        return true;
    }
}


public partial class RangeWeaponController
{
    private void HandleLookTriggered(Transform creatureTransform, Vector2 input)
    {
        if (input.IsNearlyZero())
        {
            return;
        }

        Vector2 lookDirection = (Utls.GetMouseWorldPosition() - creatureTransform.position).normalized;
        if (lookDirection.IsNearlyZero())
        {
            return;
        }

        // 상각 제한
        if (lookDirection.x < lookDeadZone && lookDirection.x > lookDeadZone * -1f)
        {
            return;
        }

        weapon.LookDirection = lookDirection;

        weaponRotationPivot.right = weapon.LookDirection;
        weaponSocket.rotation = weaponRotationPivot.rotation;

        OnHandleLookDirectionChanged?.Invoke(weapon.LookDirection);
    }

    private void HandleFireTriggered()
    {
        if (!TryFire())
        {
            return;
        }

        projectilePool?.Get();
        OnHandleFireTriggered?.Invoke(weapon.CurrAmmo, weapon.MaxAmmo);
    }

    private void HandleReloadTriggered()
    {
        if (!TryReload()) return;

        OnHandleReloadTriggered?.Invoke(weapon.CurrAmmo, weapon.MaxAmmo);
    }
}
