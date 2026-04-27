using UnityEngine;

public abstract class WeaponController : MonoBehaviour
{
    public bool IsLeft { get; set; }

    protected virtual void OnValidate()
    {
        Init();
    }

    protected virtual void Awake()
    {
        Init();
    }

    protected virtual void Init() {}
}
