using UnityEngine;

public class WorldImageResolutionMatcher : MonoBehaviour
{
    private Vector3 initialScale;

    private void Awake()
    {
        initialScale = transform.localScale;
    }

    private void OnEnable()
    {
        UIManager.GetInstance().OnResolutionChanged.AddListener(UpdateScale);

    }

    private void OnDisable()
    {
        UIManager.GetInstance().OnResolutionChanged.RemoveListener(UpdateScale);
    }

    private void Start()
    {
        // 초기 해상도에 맞춰 1회 실행
        UpdateScale(Screen.width, Screen.height);
    }

    private void UpdateScale(float width, float height)
    {
        Vector2 referenceResolution = UIManager.GetInstance().GetReferenceResolution();
        float scaleFactor = height / referenceResolution.y;

        transform.localScale = initialScale * scaleFactor;
    }
}