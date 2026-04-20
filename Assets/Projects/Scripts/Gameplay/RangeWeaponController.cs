using System;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.TextCore.Text;

[RequireComponent(typeof(RangeWeapon))]
[RequireComponent(typeof(RangeWeaponView))]
public class RangeWeaponController : MonoBehaviour
{
    [SerializeField] private Transform weaponSocket;

    [SerializeField] private Transform weaponRotationPivot;

    [SerializeField] private RangeWeapon weapon;

    [SerializeField] private RangeWeaponView view;

    private ObjectPool<Projectile> projectilePool;

    [SerializeField] private float lookDeadZone = 0.1f;

    [Header("External")]
    [SerializeField] private PlayerController playerController;




    public event Action<Vector2> OnLookDirectionChanged;

    public event Action<int, int> OnAmmoChanged;





    private void Awake()
    {
        Init();
    }

    private void OnValidate()
    {
        Init();
    }

    private void Start()
    {
        if (weapon == null)
        {
            return;
        }

        OnAmmoChanged?.Invoke(weapon.CurrAmmo, weapon.MaxAmmo);
    }

    private void Init()
    {
        weapon = Utls.FindComponent<RangeWeapon>(gameObject);
        playerController = Utls.FindComponentByTag<PlayerController>(UnityConstant.Tags.Player);
        view = Utls.FindComponent<RangeWeaponView>(gameObject);

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

    private void HandleLookTriggered(Transform creatureTransform, Vector2 input)
    {
        if (input.IsNearlyZero())
        {
            return;
        }

        // 플레이어에서 크로스헤어까지의 벡터
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

        weaponRotationPivot.right = lookDirection;
        weaponSocket.rotation = weaponRotationPivot.rotation;

        view.UpdateLookDirection(lookDirection);
        view.UpdateFlip();

        OnLookDirectionChanged?.Invoke(lookDirection);
    }

    private void HandleFireTriggered()
    {
        if (!TryFire())
        {
            return;
        }

        projectilePool?.Get();
        OnAmmoChanged?.Invoke(weapon.CurrAmmo, weapon.MaxAmmo);
    }

    private void HandleReloadTriggered()
    {
        if (!TryReload())
        {
            return;
        }

        OnAmmoChanged?.Invoke(weapon.CurrAmmo, weapon.MaxAmmo);
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
        if (weapon.CurrAmmo <= 0)
        {
            return false;
        }

        weapon.CurrAmmo--;
        return true;
    }

    private bool TryReload()
    {
        if (weapon.CurrAmmo >= weapon.MaxAmmo)
        {
            return false;
        }

        weapon.CurrAmmo = weapon.MaxAmmo;
        return true;
    }
}
