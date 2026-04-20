using System;
using UnityEngine;

[RequireComponent(typeof(Weapon))]
[RequireComponent(typeof(RangeWeaponController))]
public class WeaponPresenter : MonoBehaviour
{
    [SerializeField, ReadOnly] private Weapon weapon;

    [SerializeField, ReadOnly] private WeaponIndicator weaponIndicator;

    [Header("Internal")]

    [SerializeField, ReadOnly] private RangeWeaponController weaponController;



    public event Action<int, int> OnAmmoChanged;




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
        weapon = Utls.FindComponent<Weapon>(gameObject);

        var gameObjects = GameObject.FindGameObjectsWithTag(UnityConstant.Tags.WeaponUI);
        for (int i = 0; i < gameObjects.Length; i++)
        {
            weaponIndicator = Utls.FindComponent<WeaponIndicator>(gameObjects[i]);
        }

        weaponController = Utls.FindComponent<RangeWeaponController>(gameObject);
    }

    private void OnEnable()
    {
        weaponController.OnAmmoChanged += OnAmmoChanged;
    }

    private void OnDisable()
    {
        weaponController.OnAmmoChanged -= OnAmmoChanged;
    }
}
