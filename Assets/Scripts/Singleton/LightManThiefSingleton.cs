using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public enum GameState { Loading, InGame, InGameAlert, OnBribeDialogs, Pause }

/// <summary>
/// General purposes singleton
/// </summary>
public class LightManThiefSingleton : BASESingleton<LightManThiefSingleton>
{
    #region Constructor

    protected LightManThiefSingleton() { }

    #endregion

    #region Fields and properties

    private bool isGameStarted = false;
    private LevelName actualGameLevel;
    private GameState actualGameState;
    private LoadingCanvasController loadingCanvasController;

    public static GameState ActualGameState => Instance.actualGameState;

    #endregion

    #region Public Methods

    /// <summary>
    /// Start game in general and stores loading canvas controller data
    /// </summary>
    /// <param name="loadingCanvasController">Loading canvas controller reference</param>
    public static void InitGame(LoadingCanvasController loadingCanvasController)
    {
        if (Instance.isGameStarted)
            return;

        Instance.isGameStarted = true;
        Instance.loadingCanvasController = loadingCanvasController;
        Instance.LoadLevelAsync(LevelName.GameScene);
    }

    public static void EndGame()
    {
        Instance.loadingCanvasController.EndGame();
        SceneManager.UnloadSceneAsync((int)LevelName.GameScene);
    }

    /// <summary>
    /// Load a required level
    /// </summary>
    /// <param name="levelToLoad">Level required to load</param>
    public void LoadLevelAsync(LevelName levelToLoad)
    {
        actualGameState = GameState.Loading;
        List<AsyncOperation> loadingSceneOperations = new List<AsyncOperation>();

        if ((int)actualGameLevel > 0)
            loadingSceneOperations.Add(SceneManager.UnloadSceneAsync((int)actualGameLevel));

        loadingSceneOperations.Add(SceneManager.LoadSceneAsync((int)levelToLoad, LoadSceneMode.Additive));
    }

    /// <summary>
    /// Action that execures after fully load level
    /// </summary>
    public void OnFinishLoadingLevel(Action afterLoadingLevelAction = null)
    {
        SetLoadingBackground(false);
        actualGameState = GameState.InGame;
        afterLoadingLevelAction?.Invoke();
    }

    /// <summary>
    /// Show or hide loading background image
    /// </summary>
    /// <param name="state">load state to be set for background</param>
    public void SetLoadingBackground(bool state, Action actionAfterFading = null)
    {
        actualGameState = GameState.Loading;

        Action actionAfterImageFading = null;
        actionAfterImageFading += actionAfterFading;

        if (!state)
            actionAfterImageFading += () => { actualGameState = GameState.InGame; };

        loadingCanvasController.SetFadeAnimState(state, actionAfterImageFading);
    }

    public void SetPause() => actualGameState = GameState.Pause;

    public static void SetDialogGameState(bool setActive) => Instance.actualGameState = setActive ? GameState.OnBribeDialogs : GameState.InGame;

    public static bool GameplayIsRunning => ActualGameState == GameState.InGame || ActualGameState == GameState.InGameAlert;

    #endregion
}
