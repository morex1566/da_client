using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    private Transform target;

    [Header("Setup")]
    [SerializeField] private float followSpeed = 5f;
    [SerializeField] private Vector3 offset = new Vector3(0f, 3f, -5f);

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        GameObject player = GameObject.FindGameObjectWithTag(UnityConstant.Tags.Player);

        if (player == null)
        {
            Debug.LogError("Player 태그를 가진 오브젝트를 찾을 수 없습니다.");
            return;
        }

        target = player.transform;
    }

    private void LateUpdate()
    {
        if (target == null) return;

        UpdateMovement();
    }

    private void UpdateMovement()
    {
        Vector3 targetPosition = target.position + offset;

        transform.position = Vector3.Lerp
        (
            transform.position,
            targetPosition,
            followSpeed * Time.deltaTime
        );
    }
}