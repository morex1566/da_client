using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private float followSpeed = 10f;
    [SerializeField] private Vector3 positionOffset = new Vector3(0f, 5f, -8f);
    [SerializeField] private Vector3 rotationOffset = new Vector3(0f, 5f, -8f);



    private Transform target;



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
        target = player.transform;
        transform.position = target.position + positionOffset;
        transform.rotation = Quaternion.Euler(rotationOffset);
    }

    private void LateUpdate()
    {
        UpdateMovement();
        UpdateRotation();
    }

    private void UpdateMovement()
    {
        Vector3 destination = target.position + positionOffset;

        transform.position = Vector3.Lerp(transform.position, destination, followSpeed * Time.deltaTime);
    }

    private void UpdateRotation()
    {
        transform.rotation = Quaternion.Euler(rotationOffset);
    }
}