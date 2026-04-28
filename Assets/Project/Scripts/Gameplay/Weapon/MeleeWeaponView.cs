using UnityEngine;

[DisallowMultipleComponent]
public class MeleeWeaponView : MonoBehaviour
{
    [Header("External")]
    [SerializeField, ReadOnly] private PlayerController playerController = null;
    [SerializeField, ReadOnly] private PlayerView playerView = null;

    [SerializeField] private Animator animator = null;

    [SerializeField] private SpriteRenderer spriter = null;


    private void OnValidate()
    {
        Init();
    }

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        animator = Utls.FindComponent<Animator>(gameObject);
        spriter = Utls.FindComponent<SpriteRenderer>(gameObject);

        if (Application.isPlaying)
        {
            playerController = Utls.FindComponentByTag<PlayerController>(UnityConstant.Tags.Player);
            playerView = Utls.FindComponentByTag<PlayerView>(UnityConstant.Tags.Player);
        }
    }

    private void Update()
    {
        UpdateFlip();
        UpdateShow();
    }

    public void UpdateFlip()
    {
        if (Mathf.Abs(playerController.CurrLookDirection.x) < playerView.LookDeadZone)
        {
            return;
        }

        spriter.flipX = playerController.CurrLookDirection.x < 0f;
    }

    // 특정 상태에서는 랜더링 비활성화
    public void UpdateShow()
    {
        if (playerController.CurrentStateType == PlayerStateType.ROLL || playerController.CurrentStateType == PlayerStateType.DEAD)
        {
            spriter.enabled = false;
        }

        spriter.enabled = true;
    }
}
