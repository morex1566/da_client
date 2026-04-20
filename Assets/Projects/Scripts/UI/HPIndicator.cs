using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class HPIndicator : MonoBehaviour
{
    [SerializeField] private PlayerPresenter presenter;

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
        presenter = Utls.FindComponentByTag<PlayerPresenter>(UnityConstant.Tags.Player);
    }


    private void OnEnable()
    {
        presenter.OnHpChanged += UpdateHpIndicator;
    }

    private void OnDisable()
    {
        presenter.OnHpChanged -= UpdateHpIndicator;
    }

    private void UpdateHpIndicator(float currentHp, float maxHp)
    {
        hpBar.value = currentHp / maxHp;
        hpText.text = $"{currentHp} / {maxHp}";
    }
}