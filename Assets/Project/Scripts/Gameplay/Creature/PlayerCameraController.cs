using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private float followSpeed = 10f;
    [SerializeField] private Vector3 positionOffset = new Vector3(0f, 5f, -8f);
    [SerializeField] private Vector3 rotationOffset = new Vector3(0f, 5f, -8f);
    [SerializeField, ReadOnly] private Transform target;

    private void OnValidate()
    {
        Init();
    }

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        GameObject player = GameObject.FindGameObjectWithTag(UnityConstant.Tags.Player);
        if (player)
        {
            target = player.transform;

            // 초기 위치 설정 시에도 스냅 적용
            transform.position = target.position + positionOffset;
            transform.rotation = Quaternion.Euler(rotationOffset);
        }
    }

    private void LateUpdate()
    {
        if (target == null)
        {
            return; // 추적 대상이 없으면 카메라 갱신 불가
        }

        UpdateMovement();
        UpdateRotation();
    }

    private void UpdateMovement()
    {
        Vector3 destination = target.position + positionOffset;

        // 부드러운 이동 계산
        Vector3 nextPos = Vector3.Lerp(transform.position, destination, followSpeed * Time.deltaTime);
        transform.position = nextPos;
    }

    private void UpdateRotation()
    {
        transform.rotation = Quaternion.Euler(rotationOffset);
    }
}
