using UnityEngine;

public abstract class CreatureController : MonoBehaviour
{
    [field: SerializeField] public CreatureData Data { get; set; } = null;

    public Vector3 CurrMoveDirection { get; set; } = Vector3.right;

    public Vector3 PrevMoveDirection { get; set; } = Vector3.right;

    public Vector3 CurrLookDirection { get; set; } = Vector3.right;

    public Vector3 PrevLookDirection { get; set; } = Vector3.right;

    public float CurrentHp { get; set; } = 0f;

    public float CurrentSp { get; set; } = 0f;

    public bool IsLeft { get; set; } = false;


    protected virtual void OnValidate()
    {
        Init();
    }

    protected virtual void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {
        if (Data == null)
        {
            return; // ScriptableObject 데이터가 연결되기 전에는 런타임 수치 초기화 보류
        }

        CurrentHp = Data.MaxHp;
        CurrentSp = Data.MaxSp;
    }
}
