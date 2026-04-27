using UnityEngine;

[CreateAssetMenu(fileName = "SO_Projectile", menuName = "Scriptable Objects/Projectile")]
public class ProjectileData : ScriptableObject
{
    public float Speed;

    public float Damage;

    public float Lifetime;
}
