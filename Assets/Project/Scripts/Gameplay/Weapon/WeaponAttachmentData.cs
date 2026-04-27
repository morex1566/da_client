using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponAttachmentData : ScriptableObject
{
    [Header("Setup")]
    [field: SerializeField] public int Price { get; set; } = 0;
    [field: SerializeField] public WeaponAttachmentType Type { get; set; }
    [field: SerializeField] public Vector3 Offset { get; set; }

    [Header("Sprite")]
    [field: SerializeField] public Sprite Sprite { get; set; } = null;

    [Header("Slots")]
    [field: SerializeField] public List<WeaponAttachmentSlot> Slots { get; set; } = new();
}
