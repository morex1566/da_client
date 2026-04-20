using TMPro;
using UnityEngine;

public class WeaponIndicator : MonoBehaviour
{
    [SerializeField] private WeaponPresenter presenter;

    [SerializeField] private TextMeshProUGUI ammoText;




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
        presenter = Utls.FindComponentByTag<WeaponPresenter>(UnityConstant.Tags.Weapon);
    }

    private void OnEnable()
    {
        presenter.OnAmmoChanged += UpdateAmmoText;
    }

    private void OnDisable()
    {
        presenter.OnAmmoChanged -= UpdateAmmoText;
    }

    private void UpdateAmmoText(int currentAmmo, int maxAmmo)
    {
        ammoText.text = $"{currentAmmo}/{maxAmmo}";
    }
}