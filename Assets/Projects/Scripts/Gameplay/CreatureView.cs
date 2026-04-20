using System;
using UnityEngine;

[Serializable]
public class CreatureView
{
    [SerializeField] private Animator animator = null;

    [SerializeField] private SpriteRenderer spriter = null;

    private float lookDeadZone = 0.1f;

    private Vector2 currLookDirection = Vector2.right;

    private Vector2 prevLookDirection = Vector2.right;


    public Vector2 CurrLookDirection => currLookDirection;

    public Vector2 PreviousLookDirection => prevLookDirection;




    public void Init(Animator animator, SpriteRenderer spriter)
    {
        this.animator = animator;
        this.spriter = spriter;
    }

    public void UpdateLookDirection(Transform creatureTransform, Vector2 input)
    {
        if (input.IsNearlyZero())
        {
            return;
        }

        prevLookDirection = currLookDirection;
        currLookDirection = (Utls.GetMouseWorldPosition() - creatureTransform.position).normalized;
    }

    // 마우스 입력 시 콜, 크로스헤어가 플레이어 기준 좌/우 위치에 따라 플립
    public void UpdateFlip()
    {
        if (spriter == null)
        {
            return;
        }

        spriter.flipX = currLookDirection.x < lookDeadZone * -1f ? true : currLookDirection.x > lookDeadZone ? false : spriter.flipX;
    }

    // 매 업데이트 마다
    public void SetAnimationParameters(bool isMoving, bool isGroggy, bool isRoll)
    {
        if (animator == null)
        {
            return;
        }

        animator.SetBool(UnityConstant.Animator.Parameters.AC_Player.Bool.IsMoving, isMoving);
        animator.SetBool(UnityConstant.Animator.Parameters.AC_Player.Bool.IsGroggy, isGroggy);
        animator.SetBool(UnityConstant.Animator.Parameters.AC_Player.Bool.IsRoll, isRoll);
    }
}
