using UnityEngine;

public enum FireMode
{
    Auto,
    SemiAuto,
    Burst
}

public class RangeWeapon : Weapon
{
    public int MaxAmmo { get; set; }

    public int CurrAmmo { get; set; }



    public RangeWeaponData Data => data as RangeWeaponData;



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
        RangeWeaponData rangeWeaponData = data as RangeWeaponData;
        {
            MaxAmmo = rangeWeaponData.MaxAmmo;
            CurrAmmo = MaxAmmo;
        }
    }
}
