using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ImageFader : MonoBehaviour
{
    public enum FadeOption
    {
        FadeIn,
        FadeOut
    }

    public enum ExecutionType
    { 
        Instant,
        Invoke
    }

    [SerializeField] private Image image;

    // Fade되는데 걸리는 시간
    [SerializeField] private float duration = 2f;

    [SerializeField] private FadeOption fadeOption = FadeOption.FadeOut;

    [SerializeField] private ExecutionType executionType = ExecutionType.Instant;


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

    public void StartFade(Action onComplete = null)
    {
        if (image == null)
        {
            return;
        }

        // 투명도를 0으로 변경
        switch (fadeOption)
        {
            case FadeOption.FadeIn:
                fading = image.DOFade(1f, duration).OnComplete(() => onComplete?.Invoke());
                break;

            case FadeOption.FadeOut:
                fading = image.DOFade(0f, duration).OnComplete(() => onComplete?.Invoke());
                break;

            default:
                break;
        }
    }

    public void StopFade()
    {
        image.DOKill();
        image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);
    }
}
