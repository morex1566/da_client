using UnityEngine;

public interface IWeapon
{
    
}

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] protected WeaponData data = null;

    public bool IsLeft { get; set; }
}
