using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;

/// <summary>
/// In game controller singleton
/// </summary>
public class InGameController : BASESingleton<InGameController>
{
    #region Constructor

    protected InGameController() {}

    #endregion

    #region Fields and properties

    [Header("External Controllers")]
    [SerializeField]
    private InGameUIController uiController;
    [SerializeField]
    private TickSystem tickSystem;
    [SerializeField]
    private PlayerController playerController;
    [SerializeField]
    private GlobalLightController globalLightController;
    [SerializeField]
    private CinemachineConfiner cameraConfiner;

    [Space(5)]
    [Header("Actual level references")]
    [SerializeField]
    private List<LevelData> gameLevels;

    private LevelData actualLevelData;
    private int actualLevelIndex;
    private LevelController actualLevelController;

    private bool gameEnded;

    public Action onGameOverAction;
    public Action onLevelFinishedAction;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        gameEnded = false;
        actualLevelIndex = 0;
        SetNewLevelData();
        SetupGame();
        onGameOverAction += () => { StartCoroutine(ResetLevel()); };
        onLevelFinishedAction += () => { uiController.ScorePanel.OpenPanel(); StartCoroutine(GoToNextLevel()); };
    }

    #endregion

    #region Public Methods

    public void SetFlashLightAttack(Vector3 lightAttackPosition)
    {
        Instance.uiController.OnUsedFlashBomb();
        Instance.actualLevelController.ActivateLightAttack(lightAttackPosition, () => { Instance.playerController.canUseFlashBombs = true; });
    }

    public void SetNewLightState(bool setPlayerLightDetected)
    {
        Instance.globalLightController.SetLightAnimationState(setPlayerLightDetected);
    }

    public void SetBribeDialog(DialogScriptableObject dialog, Action onEnemyAceptedBribe, Action onEnemyCanceledBribe)
    {
        LightManThiefSingleton.SetDialogGameState(true);
        uiController.SetBribeUIActive(dialog, onEnemyAceptedBribe, onEnemyCanceledBribe);
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Setup game after loading main game data
    /// </summary>
    private void SetupGame()
    {
        SetNewLightState(false);
        cameraConfiner.m_BoundingShape2D = actualLevelController.LevelBoundary;
        actualLevelData.PlayerStatsData.SetStartingPlayerStats();
        actualLevelData.UpdateLevelMainData(actualLevelController.LevelItemsData);
        uiController.InitController(actualLevelData);
        actualLevelController.OnStartLevel(new List<IObserver>() { uiController }, OnLightNearUpdate);
        LightManThiefSingleton.Instance.OnFinishLoadingLevel(tickSystem.StartTickingSystem);
    }

    /// <summary>
    /// Reset level including original data
    /// </summary>
    private IEnumerator ResetLevel()
    {
        Action onResetLevelAction = () => { playerController.transform.position = actualLevelData.PlayerInitialPosition; actualLevelController.OnRestartLevel(); };
        tickSystem.StopTicker();
        StopLevelGameplay(onResetLevelAction);
        yield return new WaitForSeconds(1.5f);
        SetupGame();
    }

    /// <summary>
    /// Load next level or set game finished screen
    /// </summary>
    private IEnumerator GoToNextLevel()
    {
        LightManThiefSingleton.Instance.SetPause();
        tickSystem.StopTicker();
        yield return new WaitUntil(uiController.ScorePanelClosed);

        StopLevelGameplay(actualLevelController.OnFinishLevel);
        yield return new WaitForSeconds(1f);

        actualLevelIndex++;
        SetNewLevelData();
        yield return new WaitForSeconds(1f);

        if (!gameEnded)
            SetupGame();
        else
            LightManThiefSingleton.EndGame();
    }

    /// <summary>
    /// Set new level data according actual level
    /// </summary>
    private void SetNewLevelData()
    {
        if (actualLevelIndex > gameLevels.Count - 1)
        {
            gameEnded = true;
            return;
        }

        actualLevelData = gameLevels[actualLevelIndex];
        actualLevelController = Instantiate(actualLevelData.LevelPrefab, transform);
        playerController.transform.position = actualLevelData.PlayerInitialPosition;
    }

    /// <summary>
    /// Set loading screen and stops any gameplay
    /// </summary>
    private void StopLevelGameplay(Action afterFadeAction = null)
    {
        LightManThiefSingleton.Instance.SetLoadingBackground(true, afterFadeAction);
    }

    private void OnLightNearUpdate(bool isNear)
    {
        uiController.SetEnableStateToAttackButton(ButtonAttack.FlashBomb, isNear);
        playerController.canUseFlashBombs = isNear;
    }

    #endregion
}
