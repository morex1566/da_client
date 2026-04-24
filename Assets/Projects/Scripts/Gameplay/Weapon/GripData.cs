using UnityEngine;

[CreateAssetMenu(fileName = "GripData", menuName = "Scriptable Objects/Weapon Attachment/Grip")]
public class GripData : WeaponAttachmentData
{
    [Header("Setup")]
    [field: SerializeField] public float MoveSpeedRate { get; set; } = 1f;

}
