using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

[RequireComponent(typeof(VideoPlayer))]
[RequireComponent(typeof(RawImage))]
[RequireComponent(typeof(RectTransform))]
public class ImageVideoRenderer : MonoBehaviour
{
    [SerializeField] private VideoPlayer player;

    [SerializeField] private RawImage output;

    [SerializeField] private RectTransform rect;

    private RenderTexture renderTexture;

    private void Awake()
    {
        player = GetComponent<VideoPlayer>();
        output = GetComponent<RawImage>();
        rect = GetComponent<RectTransform>();
    }

    void Start()
    {
        StartCoroutine(DelayedInit());
    }

    private System.Collections.IEnumerator DelayedInit()
    {
        yield return null;
        Init();
    }

    public void Init()
    {
        if (renderTexture != null)
        {
            renderTexture.Release();
            Destroy(renderTexture);
        }

        player.renderMode = VideoRenderMode.RenderTexture;

        int width = Mathf.Max(1, (int)rect.rect.width);
        int height = Mathf.Max(1, (int)rect.rect.height);

        Debug.Log($"[ImageVideoRenderer] Init with size: {width}x{height}");

        renderTexture = new RenderTexture(width, height, 0);
        renderTexture.name = $"{nameof(VideoPlayer)}";
        renderTexture.Create();

        player.targetTexture = renderTexture;
        output.texture = renderTexture;

        player.Play();
    }

    private void OnDestroy()
    {
        if (renderTexture != null)
        {
            renderTexture.Release();
            Destroy(renderTexture);
            renderTexture = null;
        }
    }
}
