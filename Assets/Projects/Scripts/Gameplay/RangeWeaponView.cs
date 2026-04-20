using UnityEngine;

[DisallowMultipleComponent]
public class RangeWeaponView : MonoBehaviour
{
    [SerializeField] private Animator animator = null;

    [SerializeField] private SpriteRenderer spriter = null;

    [SerializeField] private float lookDeadZone = 0.1f;




    private Vector2 lookDirection = Vector2.right;




    private void OnValidate()
    {
        Init();
    }

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        animator = Utls.FindComponent<Animator>(gameObject);
        spriter = Utls.FindComponent<SpriteRenderer>(gameObject);
    }

    public void UpdateLookDirection(Vector2 direction)
    {
        if (direction.IsNearlyZero())
        {
            return;
        }

        lookDirection = direction.normalized;
    }

    // 마우스 입력 시 콜, 크로스헤어가 플레이어 기준 좌/우 위치에 따라 플립
    public void UpdateFlip()
    {
        if (spriter == null)
        {
            return;
        }

        spriter.flipY = lookDirection.x < lookDeadZone * -1f ? true : lookDirection.x > lookDeadZone ? false : spriter.flipY;
    }
}
