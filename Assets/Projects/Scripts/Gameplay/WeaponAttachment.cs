using UnityEngine;

public abstract class WeaponAttachment : MonoBehaviour
{
    [field: SerializeField] private WeaponAttachmentData Data { get; set; } = null;
}
