using UnityEngine;

[System.Serializable]
public class RangeWeaponMovement
{
    [Header("Aim")]
    private float lookDeadZone = 0.1f;

    [Header("Kick")]
    public float kickbackDistance = 0.1f;
    public float kickbackSpeed = 20f;

    [Header("Rotation")]
    public float recoilAngle = 5f;
    public float rotationSpeed = 20f;

    [Header("Recovery")]
    public float positionReturnSpeed = 12f;
    public float rotationReturnSpeed = 10f;

    [Header("Clamp")]
    public float maxKickbackDistance = 0.25f;
    public float maxRecoilAngle = 15f;

    private Transform weaponRotationPivot = null;

    private Transform weaponSocket = null;

    private Vector2 currLookDirection = Vector2.right;

    private Vector2 prevLookDirection = Vector2.right;

    public Vector2 CurrentLookDirection => currLookDirection;

    public Vector2 PreviousLookDirection => prevLookDirection;

    public void Init(Transform weaponRotationPivot, Transform weaponSocket)
    {
        this.weaponRotationPivot = weaponRotationPivot;
        this.weaponSocket = weaponSocket;
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

    public void UpdateAimRotation(Transform creatureTransform, Vector2 input)
    {
        if (currLookDirection.x >= -lookDeadZone && currLookDirection.x <= lookDeadZone)
        {
            return;
        }

        weaponRotationPivot.right = currLookDirection;
    }

    // 사격 시 콜, 무기가 튕겨 올라감
    private void ApplyRecoil()
    {
    }
    
    // 사격 시 콜, 무기가 뒤로 밀림
    private void ApplyKickback()
    {
    }

}
