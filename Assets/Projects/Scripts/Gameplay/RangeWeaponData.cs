using UnityEngine;

[CreateAssetMenu(fileName = "RangeWeaponData", menuName = "Scriptable Objects/RangeWeaponData")]
public class RangeWeaponData : WeaponData
{
    [Header("Projectile")]
    [SerializeField] public GameObject projectilePf = null;



    [Header("Audio")]
    public AudioClip Fire;

    public AudioClip Cocking;

    public AudioClip Ejecting;

    public AudioClip inserting;



    [Header("Setup")]
    public int MaxAmmo;

    public FireMode Mode;

    public float ReloadTime;

    public float Damage;

    public float rpm;
}