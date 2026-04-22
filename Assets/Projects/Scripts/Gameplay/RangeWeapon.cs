using UnityEngine;

public class RangeWeapon : Weapon
{
    public int MaxAmmo { get; set; }

    public int CurrAmmo { get; set; }

    public Vector2 LookDirection { get; set; } = Vector2.right;

    public RangeWeaponStateType StateType { get; set; } = RangeWeaponStateType.Idle;




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
