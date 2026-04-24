using UnityEngine;

[CreateAssetMenu(fileName = "AssemblyData", menuName = "Scriptable Objects/Weapon Attachment/Assembly")]
public class AssemblyData : WeaponAttachmentData
{
    [Header("Setup")]
    [field: SerializeField] public WeaponAttachmentBodyType BodyType { get; set; } = WeaponAttachmentBodyType.None;
    [field: SerializeField] public float RPM { get; set; } = 200f;
    [field: SerializeField] public float DamageRate { get; set; } = 1f;
}
