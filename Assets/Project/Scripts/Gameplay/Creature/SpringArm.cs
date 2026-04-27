using System;
using UnityEngine;

[Serializable]
public class SpringArm
{
    [SerializeField, Min(0f)] private float length = 2f;

    [SerializeField] private Transform pivotTransform = null;

    [SerializeField] private Transform socketTransform = null;



    public float Length => length;

    public Transform PivotTransform => pivotTransform;

    public Transform SocketTransform => socketTransform;



    public void Update(Vector3 lookDirection)
    {
        socketTransform.position = pivotTransform.position + lookDirection * length;

        float aimAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;

        // 피벗은 실제 조준 방향 그대로
        pivotTransform.rotation = Quaternion.Euler(0f, 0f, aimAngle);
        socketTransform.rotation = Quaternion.Euler(0f, 0f, aimAngle);

        //// 스프라이트가 flip되면 로컬 기준 회전은 반대로 보정
        //float visualAngle = aimAngle;
        //if (lookDirection.x < 0f)
        //{
        //    visualAngle += 180f;
        //    visualAngle *= -1f;
        //}

    }
}