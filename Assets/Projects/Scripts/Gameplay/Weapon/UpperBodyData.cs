using UnityEngine;

[CreateAssetMenu(fileName = "UpperBodyData", menuName = "Scriptable Objects/Weapon Attachment/UpperBody")]
public class UpperBodyData : WeaponAttachmentData
{
    [field: SerializeField] public float RecoilRate { get; set; } = 1f;
}