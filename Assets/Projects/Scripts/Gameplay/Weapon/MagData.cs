using UnityEngine;

[CreateAssetMenu(fileName = "MagData", menuName = "Scriptable Objects/Weapon Attachment/Mag")]
public class MagData : WeaponAttachmentData
{
    [Header("Setup")]
    [field: SerializeField] public int MaxAmmo { get; set; } = 0;
    [field: SerializeField] public float ReloadTimeRate { get; set; } = 0f;

    [Header("SFX")]
    [field: SerializeField] public AudioClip MagInSfx { get; set; } = null;
    [field: SerializeField] public AudioClip MagOutSfx { get; set; } = null;
    [field: SerializeField] public AudioClip MagDropSfx { get; set; } = null;
}
