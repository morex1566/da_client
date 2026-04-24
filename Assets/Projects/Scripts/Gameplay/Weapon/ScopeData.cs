using UnityEngine;

[CreateAssetMenu(fileName = "ScopeData", menuName = "Scriptable Objects/Weapon Attachment/Scope")]
public class ScopeData : WeaponAttachmentData
{
    [Header("Setup")]
    [field: SerializeField] public float AccuracyRate { get; set; } = 1f;
}