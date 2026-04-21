using UnityEngine;

public interface IWeapon
{
    
}

public abstract class Weapon : MonoBehaviour
{
    [field : SerializeField] protected WeaponData Data { get; set; } = null;

    public bool IsLeft { get; set; }
}
