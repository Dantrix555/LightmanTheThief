using System;
using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

/// <summary>
/// Base controller player class
/// </summary>
public class PlayerController : BaseCharacter, MainGameInputs.IPlayerMapActions
{
    #region Fields and Properties

    [SerializeField]
    private PlayerWhipController playerWhip;

    [HideInInspector]
    public bool canUseFlashBombs;

    private MainGameInputs playerInputs;
    private Vector2 movingDirection;
    private PlayerStats playerStats;

    private Vector3 nearLightPostPosition;

    public Vector3 NearLightPostPosition { set => nearLightPostPosition = value; }

    #endregion

    #region Unity Methods

    private void Awake()
    {
        playerInputs = new MainGameInputs();
        playerInputs.PlayerMap.SetCallbacks(this);
        ConfigureCharacter();
    }

    private void OnEnable()
    {
        playerInputs.Enable();
    }

    private void OnDisable()
    {
        playerInputs.Disable();
    }

    private void FixedUpdate()
    {
        if (!LightManThiefSingleton.GameplayIsRunning)
        {
            MoveCharacter(Vector2.zero);
            return;
        }

        MoveCharacter(movingDirection);
        DetectInteractablesAround();
    }

    #endregion

    #region Base Character Inheritance

    protected override void ConfigureCharacter()
    {
        playerStats = characterStatsData as PlayerStats;
        actualCharacterSpeed = playerStats.CharacterBaseSpeed;
        detectionRadius = characterStatsData.CharacterDetectionRadius;

        playerWhip.SetupInteractable();
    }

    public override void SetCharacterDefeated()
    {
        characterAnimator.SetTrigger("Death");
    }

    #endregion

    #region Input System Inheritance

    public void OnMove(InputAction.CallbackContext context)
    {
        movingDirection = context.ReadValue<Vector2>();
    }

    public void OnUseFlashBomb(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed && playerStats.FlashShots > 0 && canUseFlashBombs
            && LightManThiefSingleton.GameplayIsRunning)
        {
            InGameController.Instance.SetFlashLightAttack(nearLightPostPosition);
            canUseFlashBombs = false;
        }
    }

    public void OnBribe(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed && LightManThiefSingleton.GameplayIsRunning)
            playerWhip.UseWhip(transform);
    }

    #endregion
}
