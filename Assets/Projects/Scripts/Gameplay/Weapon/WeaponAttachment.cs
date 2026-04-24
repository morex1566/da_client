using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WeaponAttachmentSlot
{
    [field: SerializeField] public WeaponAttachmentType Type { get; set; }
    [field: SerializeField] public WeaponAttachmentData Data { get; set; }
    [field: SerializeField] public Vector3 Offset { get; set; }
}

public class WeaponAttachment : MonoBehaviour
{
    [field: SerializeField] private WeaponAttachmentData Data { get; set; } = null;

    [field: SerializeField] public List<WeaponAttachmentSlot> Slots { get; set; } = new();



    private void OnValidate()
    {
        
    }

    private void Awake()
    {
        
    }

    private void Init()
    {

    }
}
