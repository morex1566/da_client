using UnityEngine;

[CreateAssetMenu(fileName = "MuzzleData", menuName = "Scriptable Objects/Weapon Attachment/Muzzle")]
public class MuzzleData : WeaponAttachmentData
{
    [Header("Setup")]
    [field: SerializeField] public float RecoilRate { get; set; } = 1f;

    [Header("SFX")]
    [field: SerializeField] public AudioClip FireSfxOverride { get; set; } = null;
}
