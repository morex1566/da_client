using UnityEngine;

[CreateAssetMenu(fileName = "RangeWeaponData", menuName = "Scriptable Objects/RangeWeaponData")]
public class WeaponAmmoData : ScriptableObject
{
    [field: SerializeField] public float DamageRate { get; set; } = 0f;
}
