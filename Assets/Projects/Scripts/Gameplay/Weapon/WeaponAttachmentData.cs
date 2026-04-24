using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponAttachmentData : ScriptableObject
{
    [field: SerializeField] public int Price { get; set; } = 0;

    [Header("Sprite")]
    [field: SerializeField] public Sprite Sprite { get; set; } = null;
}
