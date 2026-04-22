using UnityEngine;

public abstract class WeaponData : ScriptableObject
{
    
}

[CreateAssetMenu(fileName = "RangeWeaponData", menuName = "Scriptable Objects/RangeWeaponData")]
public class RangeWeaponData : WeaponData
{
    [SerializeField] public GameObject projectilePf = null;



    [Header("Audio")]
    public AudioClip Fire;

    public AudioClip Cocking;

    public AudioClip Ejecting;

    public AudioClip inserting;



    [Header("Setup")]
    public int MaxAmmo;

    public RangeWeaponFireModeType ModeType;

    public float ReloadTime;

    public float Damage;

    public float rpm;
}

[CreateAssetMenu(fileName = "MeleeWeaponData", menuName = "Scriptable Objects/MeleeWeaponData")]
public class MeleeWeaponData : WeaponData
{
    public float AttackCooldown = 0.2f;
}
