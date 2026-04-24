using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ImageFader : MonoBehaviour
{
    public enum ExecutionType
    { 
        Instant,
        Invoke
    }

    [SerializeField] private Image image;

    // Fade되는데 걸리는 시간
    [SerializeField] private float duration = 2f;

    [SerializeField, Range(0f, 1f)] private float endAlpha = 0f;

    [SerializeField] private ExecutionType executionType = ExecutionType.Instant;

    public UnityEvent OnFadeCompleted = new();

    private TweenerCore<Color, Color, ColorOptions> fading;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void Start()
    {
        switch (executionType)
        {
            case ExecutionType.Instant:
                StartFade();
                break;

            case ExecutionType.Invoke:
                break;

            default:
                StartFade();
                break;
        }
    }

    public void StartFade(Action onCompleted = null)
    {
        if (image == null)
        {
            return;
        }

        fading = image.DOFade(endAlpha, duration).OnComplete(() => 
        {
            onCompleted?.Invoke();
            OnFadeCompleted?.Invoke();
        });
    }

    public void StopFade()
    {
        image.DOKill();
        image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);
    }
}
