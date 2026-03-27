using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
[RequireComponent(typeof(RectTransform))]
public class ImageGyroscoper : MonoBehaviour
{
    [SerializeField] private RectTransform rect;
    [SerializeField] private float sensitivity = 1.0f;
    [SerializeField] private float smoothMoveTime = 0.2f;
    [SerializeField] private Vector2 moveLimit = new Vector2(50f, 50f);

    private Vector2 startPosition;
    private Vector2 targetPosition;
    private Vector2 currentMoveVelocity;

    private void Awake()
    {
        if (rect == null) rect = GetComponent<RectTransform>();
    }

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        UpdateMovement();
    }

    private void Init()
    {
        startPosition = rect.anchoredPosition;
        targetPosition = rect.anchoredPosition;

        moveLimit = new Vector2(rect.offsetMax.x - 1, rect.offsetMax.y - 1);
    }

    private void UpdateMovement()
    {
        if (Mouse.current == null) return;

        // 입력
        Vector2 mouseDelta = Mouse.current.delta.ReadValue();
        targetPosition += mouseDelta * sensitivity;

        // limit 범위 Clamp
        targetPosition.x = Mathf.Clamp(targetPosition.x, startPosition.x - moveLimit.x, startPosition.x + moveLimit.x);
        targetPosition.y = Mathf.Clamp(targetPosition.y, startPosition.y - moveLimit.y, startPosition.y + moveLimit.y);

        // 이동
        rect.anchoredPosition = Vector2.SmoothDamp(rect.anchoredPosition, targetPosition, ref currentMoveVelocity, smoothMoveTime);
    }
}