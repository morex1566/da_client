using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(PlayerController))]
public class PlayerPresenter : MonoBehaviour
{
    [SerializeField, ReadOnly] private Player player = null;

    [SerializeField, ReadOnly] private HPIndicator hpIndicator = null;

    [Header("Internal")]
    [SerializeField, ReadOnly] private PlayerController playerController = null;




    public event Action<float, float> OnHpChanged = null;




    private void Awake()
    {
        Init();
    }

    private void OnValidate()
    {
        Init();
    }

    private void Init()
    {
        player = Utls.FindComponent<Player>(gameObject);

        var gameObjects = GameObject.FindGameObjectsWithTag(UnityConstant.Tags.PlayerUI);
        for (int i = 0; i < gameObjects.Length; i++)
        {
            hpIndicator = Utls.FindComponent<HPIndicator>(gameObjects[i]);
        }

        playerController = Utls.FindComponent<PlayerController>(gameObject);
    }

    private void OnEnable()
    {
        playerController.OnColliderTriggered += OnHpChanged;
    }

    private void OnDisable()
    {
        playerController.OnColliderTriggered -= OnHpChanged;
    }
}
