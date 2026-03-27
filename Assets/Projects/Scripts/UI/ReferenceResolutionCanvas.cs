using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 해상도 변경 이벤트의 호출을 담당.
/// UIManager에 연결된 이벤트.
/// </summary>
[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(Canvas))]
public class ReferenceResolutionCanvas : UIBehaviour
{
    public Canvas canvas;

    public CanvasScaler canvasScaler;

    protected override void Awake()
    {
        base.Awake();

        canvas = GetComponent<Canvas>();
        canvasScaler = GetComponent<CanvasScaler>();
    }

    protected override void OnRectTransformDimensionsChange()
    {
        base.OnRectTransformDimensionsChange();

        var rectTransform = GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            var size = rectTransform.rect.size;
            
            // 싱글톤 인스턴스를 가져오되, 애플리케이션 종료 중(isQuitting)에는 새로 생성하지 않음
            var uiManager = UIManager.GetInstance();
            if (uiManager != null && uiManager.OnResolutionChanged != null)
            {
                uiManager.OnResolutionChanged.Invoke(size.x, size.y);
            }
        }
    }
}