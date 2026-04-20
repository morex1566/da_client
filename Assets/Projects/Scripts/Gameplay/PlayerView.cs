using UnityEngine;

[DisallowMultipleComponent]
public class PlayerView : MonoBehaviour
{
    [SerializeField] private Animator animator = null;

    [SerializeField] private SpriteRenderer spriter = null;

    [SerializeField] private float lookDeadZone = 0.1f;

    private Vector2 lookDirection = Vector2.right;



    public Vector2 LookDirection => lookDirection;



    public void Init()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        if (spriter == null)
        {
            spriter = GetComponent<SpriteRenderer>();
        }
    }

    public void UpdateLookDirection(Vector2 direction)
    {
        if (direction.IsNearlyZero())
        {
            return;
        }

        lookDirection = direction.normalized;
    }

    public void UpdateFlip()
    {
        if (spriter == null)
        {
            return;
        }

        spriter.flipX =
            lookDirection.x < lookDeadZone * -1f ? true :
            lookDirection.x > lookDeadZone ? false :
            spriter.flipX;
    }

    public void ApplyAnimation(bool isMoving, bool isGroggy, bool isRolling)
    {
        if (animator == null)
        {
            return;
        }

        animator.SetBool(UnityConstant.Animator.Parameters.AC_Player.Bool.IsMoving, isMoving);
        animator.SetBool(UnityConstant.Animator.Parameters.AC_Player.Bool.IsGroggy, isGroggy);
        animator.SetBool(UnityConstant.Animator.Parameters.AC_Player.Bool.IsRoll, isRolling);
    }
}
