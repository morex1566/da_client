using UnityEngine;

public abstract class WeaponAttachmentData : ScriptableObject
{
    [field: SerializeField] public int Price { get; set; } = 0;
}

[CreateAssetMenu(fileName = "WeaponMagData", menuName = "Scriptable Objects/Weapon Attachment")]
public class WeaponMagData : WeaponAttachmentData
{
    [Header("Setup")]
    [field: SerializeField] public int MaxAmmo { get; set; } = 0;
    [field: SerializeField] public float ReloadTimeRate { get; set; } = 0f;

    [Header("Sprite")]
    [field : SerializeField] public Sprite MagSprite { get; set; } = null;

    [Header("SFX")]
    [field: SerializeField] public AudioClip MagInSfx { get; set; } = null;
    [field: SerializeField] public AudioClip MagOutSfx { get; set; } = null;
    [field: SerializeField] public AudioClip MagDropSfx { get; set; } = null;
}



[CreateAssetMenu(fileName = "GripData", menuName = "Scriptable Objects/Weapon Attachment")]
public class GripData : WeaponAttachmentData
{
    [Header("Setup")]
    [field: SerializeField] public float MoveSpeedRate { get; set; } = 1f;
}



[CreateAssetMenu(fileName = "BodyData", menuName = "Scriptable Objects/Weapon Attachment")]
public class BodyData : WeaponAttachmentData
{
    [Header("Setup")]
    [field: SerializeField] public WeaponAttachmentBodyType BodyType { get; set; } = WeaponAttachmentBodyType.None;
    [field: SerializeField] public float RPM { get; set; } = 200f;
    [field: SerializeField] public float DamageRate { get; set; } = 1f;
}



[CreateAssetMenu(fileName = "MuzzleData", menuName = "Scriptable Objects/Weapon Attachment")]
public class MuzzleData : WeaponAttachmentData
{
    [Header("Setup")]
    [field: SerializeField] public float RecoilRate { get; set; } = 1f;

    [Header("SFX")]
    [field: SerializeField] public AudioClip FireSfxOverride { get; set; } = null;
}



[CreateAssetMenu(fileName = "BarrelData", menuName = "Scriptable Objects/Weapon Attachment")]
public class BarrelData : WeaponAttachmentData
{
    [Header("Setup")]
    [field: SerializeField] public float BulletSpeedRate { get; set; } = 1f;
}



[CreateAssetMenu(fileName = "ScopeData", menuName = "Scriptable Objects/Weapon Attachment")]
public class ScopeData : WeaponAttachmentData
{
    [Header("Setup")]
    [field: SerializeField] public float AccuracyRate { get; set; } = 1f;
}