using UnityEngine;

namespace TRPG.Runtime
{
    [RequireComponent(typeof(RectTransform))]
    public class SafeAreaBezelFitter : MonoBehaviour
    {
        [SerializeField] private RectTransform topBezel;
        [SerializeField] private RectTransform bottomBezel;
        [SerializeField] private RectTransform leftBezel;
        [SerializeField] private RectTransform rightBezel;

        private Rect lastSafeArea;
        private Vector2Int lastScreenSize;

        private void OnEnable()
        {
            Apply();
        }

        private void OnRectTransformDimensionsChange()
        {
            if (!Application.isPlaying) return;

            Apply();
        }

        private void Update()
        {
            Vector2Int screenSize = new Vector2Int(Screen.width, Screen.height);

            if (lastSafeArea != Screen.safeArea || lastScreenSize != screenSize)
            {
                Apply();
            }
        }

        private void Apply()
        {
            Rect safe = Screen.safeArea;
            float screenW = Screen.width;
            float screenH = Screen.height;

            if (screenW <= 0f || screenH <= 0f) return;

            lastSafeArea = safe;
            lastScreenSize = new Vector2Int(Screen.width, Screen.height);

            FitRoot();

            float safeXMin = Mathf.Clamp01(safe.xMin / screenW);
            float safeXMax = Mathf.Clamp01(safe.xMax / screenW);
            float safeYMin = Mathf.Clamp01(safe.yMin / screenH);
            float safeYMax = Mathf.Clamp01(safe.yMax / screenH);

            SetTop(safeYMax);
            SetBottom(safeYMin);
            SetLeft(safeXMin, safeYMin, safeYMax);
            SetRight(safeXMax, safeYMin, safeYMax);
        }

        private void FitRoot()
        {
            RectTransform root = (RectTransform)transform;

            root.anchorMin = Vector2.zero;
            root.anchorMax = Vector2.one;
            root.pivot = new Vector2(0.5f, 0.5f);
            root.anchoredPosition = Vector2.zero;
            root.sizeDelta = Vector2.zero;
        }

        private void SetTop(float safeYMax)
        {
            topBezel.anchorMin = new Vector2(0f, safeYMax);
            topBezel.anchorMax = new Vector2(1f, 1f);
            topBezel.pivot = new Vector2(0.5f, 1f);
            topBezel.anchoredPosition = Vector2.zero;
            topBezel.sizeDelta = Vector2.zero;
            topBezel.gameObject.SetActive(safeYMax < 1f);
        }

        private void SetBottom(float safeYMin)
        {
            bottomBezel.anchorMin = new Vector2(0f, 0f);
            bottomBezel.anchorMax = new Vector2(1f, safeYMin);
            bottomBezel.pivot = new Vector2(0.5f, 0f);
            bottomBezel.anchoredPosition = Vector2.zero;
            bottomBezel.sizeDelta = Vector2.zero;
            bottomBezel.gameObject.SetActive(safeYMin > 0f);
        }

        private void SetLeft(float safeXMin, float safeYMin, float safeYMax)
        {
            leftBezel.anchorMin = new Vector2(0f, safeYMin);
            leftBezel.anchorMax = new Vector2(safeXMin, safeYMax);
            leftBezel.pivot = new Vector2(0f, 0f);
            leftBezel.anchoredPosition = Vector2.zero;
            leftBezel.sizeDelta = Vector2.zero;
            leftBezel.gameObject.SetActive(safeXMin > 0f);
        }

        private void SetRight(float safeXMax, float safeYMin, float safeYMax)
        {
            rightBezel.anchorMin = new Vector2(safeXMax, safeYMin);
            rightBezel.anchorMax = new Vector2(1f, safeYMax);
            rightBezel.pivot = new Vector2(1f, 0f);
            rightBezel.anchoredPosition = Vector2.zero;
            rightBezel.sizeDelta = Vector2.zero;
            rightBezel.gameObject.SetActive(safeXMax < 1f);
        }
    }
}
