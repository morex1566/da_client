using UnityEngine;

[CreateAssetMenu(fileName = "SO_Player", menuName = "Scriptable Objects/Creature/Player")]
public class PlayerData : CreatureData
{
    [field: SerializeField] public float RollSpCost { get; set; } = 25f;

    [field: SerializeField] public float SpeedAccelAtRollMultiplier { get; set; } = 3f;

    [field: SerializeField] public float SpeedDecelAtRollMultiplier { get; set; } = 0.3f;



    [field: SerializeField] public float JumpForce { get; set; } = 6f;        // 처음 위로 튀는 힘

    [field: SerializeField] public float Gravity { get; set; } = -10f;        // 기본 중력

    [field: SerializeField] public float FallMultiplier { get; set; } = 2f;   // 내려올 때 더 빠르게

    [field: SerializeField] public float MaxFallSpeed { get; set; } = -25f;   // 최대 낙하 속도 제한
}
