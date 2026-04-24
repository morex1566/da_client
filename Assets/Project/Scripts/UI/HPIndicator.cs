using TMPro;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Slider))]
public class HPIndicator : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;

    [SerializeField] private Slider hpBar;

    [SerializeField] private TextMeshProUGUI hpText;



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
        playerController ??= FindFirstObjectByType<PlayerController>();
        hpBar ??= GetComponent<Slider>();
    }

    private void OnEnable()
    {
        playerController.OnColliderTriggered += UpdateHpIndicator;
    }

    private void OnDisable()
    {
        playerController.OnColliderTriggered -= UpdateHpIndicator;
    }

    private void UpdateHpIndicator(float currentHp, float maxHp)
    {
        hpBar.value = currentHp / maxHp;
        hpText.text = $"{currentHp} / {maxHp}";
    }
}
