using UnityEngine;

public enum FireMode
{
    Auto,
    SemiAuto,
    Burst
}

public enum RangeWeaponState
{
    Idle,
    Reload
}

public class RangeWeapon : Weapon
{
    public int MaxAmmo { get; set; }

    public int CurrAmmo { get; set; }

    public Vector2 LookDirection { get; set; } = Vector2.right;

    public RangeWeaponState State { get; set; } = RangeWeaponState.Idle;




    public new RangeWeaponData Data => base.Data as RangeWeaponData;



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
        RangeWeaponData rangeWeaponData = base.Data as RangeWeaponData;
        {
            MaxAmmo = rangeWeaponData.MaxAmmo;
            CurrAmmo = MaxAmmo;
        }
    }
}
