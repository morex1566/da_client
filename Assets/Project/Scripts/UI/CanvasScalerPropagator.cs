using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TRPG.Runtime
{
    public class CanvasScalerPropagator : MonoBehaviour
    {
        [Header("Reference Canvas")]
        [SerializeField] private Canvas referenceCanvas;

        private void OnValidate()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                Sync();
            }
#endif
        }

        [ContextMenu("Sync Canvas Scaler Settings")]
        public void Sync()
        {
            if (referenceCanvas == null)
            {
                Debug.LogWarning("[CanvasScalerPropagator] Reference Canvas가 없습니다.");
                return;
            }

            CanvasScaler sourceScaler = referenceCanvas.GetComponent<CanvasScaler>();
            if (sourceScaler == null)
            {
                Debug.LogWarning("[CanvasScalerPropagator] Reference Canvas에 CanvasScaler가 없습니다.");
                return;
            }

            Canvas[] canvases = GetComponentsInChildren<Canvas>(true);

            foreach (Canvas targetCanvas in canvases)
            {
                if (targetCanvas == referenceCanvas)
                    continue;

                SyncCanvasScaler(sourceScaler, targetCanvas);
            }

            Debug.Log("[CanvasScalerPropagator] CanvasScaler 설정 동기화 완료");
        }

        private void SyncCanvasScaler(CanvasScaler sourceScaler, Canvas targetCanvas)
        {
            CanvasScaler targetScaler = targetCanvas.GetComponent<CanvasScaler>();

            if (targetScaler == null)
                targetScaler = targetCanvas.gameObject.AddComponent<CanvasScaler>();

            targetScaler.uiScaleMode = sourceScaler.uiScaleMode;
            targetScaler.referenceResolution = sourceScaler.referenceResolution;
            targetScaler.screenMatchMode = sourceScaler.screenMatchMode;
            targetScaler.matchWidthOrHeight = sourceScaler.matchWidthOrHeight;
            targetScaler.referencePixelsPerUnit = sourceScaler.referencePixelsPerUnit;

            targetScaler.scaleFactor = sourceScaler.scaleFactor;
            targetScaler.physicalUnit = sourceScaler.physicalUnit;
            targetScaler.fallbackScreenDPI = sourceScaler.fallbackScreenDPI;
            targetScaler.defaultSpriteDPI = sourceScaler.defaultSpriteDPI;
            targetScaler.dynamicPixelsPerUnit = sourceScaler.dynamicPixelsPerUnit;

#if UNITY_EDITOR
            EditorUtility.SetDirty(targetScaler);
#endif
        }
    }
}