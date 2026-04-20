using TMPro;
using UnityEngine;

public class WeaponIndicator : MonoBehaviour
{
    [SerializeField] private RangeWeaponController weaponController;

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
        weaponController ??= FindFirstObjectByType<RangeWeaponController>();
    }

    private void OnEnable()
    {
        weaponController.OnHandleFireTriggered += UpdateAmmoText;
        weaponController.OnHandleReloadTriggered += UpdateAmmoText;
    }

    private void OnDisable()
    {
        weaponController.OnHandleFireTriggered -= UpdateAmmoText;
        weaponController.OnHandleReloadTriggered -= UpdateAmmoText;
    }

    private void UpdateAmmoText(int currentAmmo, int maxAmmo)
    {
        ammoText.text = $"{currentAmmo}/{maxAmmo}";
    }
}
