using System;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

/// <summary>
/// Loading Screen canvas controller
/// </summary>
public class LoadingCanvasController : MonoBehaviour
{
    #region Fields and properties
    [Header("Panels canvas group")]
    [SerializeField]
    private CanvasGroup loadingPanelCanvasGroup;
    [SerializeField]
    private CanvasGroup mainTitleCanvasGroup;
    [SerializeField]
    private CanvasGroup endingViewCanvasGroup;

    [Space(5)]
    [Header("Main UI Elements")]
    [SerializeField]
    private EventSystem eventSystem;
    [SerializeField]
    private Button startGameButton;
    [SerializeField]
    private Button endGameButton;

    private bool gameStarted;
    private MainGameInputs gameInputs;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        gameStarted = false;
        InitController();
        gameInputs = new MainGameInputs();
        gameInputs.UiMap.Accept.performed += OnButtonInputDetected;
    }

    private void OnEnable()
    {
        gameInputs.Enable();
    }

    private void OnDisable()
    {
        gameInputs.Disable();
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Set fade in or fade out anim for background image
    /// </summary>
    /// <param name="isFadingIn">Fade state (true = in) or (false = out)</param>
    /// <param name="actionAfterFade">Custom action that invokes after fade action</param>
    public void SetFadeAnimState(bool isFadingIn, Action actionAfterFade = null, CanvasGroup canvasGroupToFade = null)
    {
        if (canvasGroupToFade == null)
            canvasGroupToFade = loadingPanelCanvasGroup;

        float endValue = isFadingIn ? 1f : 0f;

        canvasGroupToFade.interactable = isFadingIn;
        canvasGroupToFade.blocksRaycasts = isFadingIn;

        canvasGroupToFade.DOFade(endValue, 1f).OnComplete(() =>
        {
            if (actionAfterFade != null)
                actionAfterFade?.Invoke();
        });
    }

    public void EndGame()
    {
        eventSystem.gameObject.SetActive(true);
        eventSystem.SetSelectedGameObject(endGameButton.gameObject);
        SetFadeAnimState(true, ()=> { SetFadeAnimState(false); }, endingViewCanvasGroup);
    }

    #endregion

    #region Private Methods
    
    private void InitController()
    {
        startGameButton.onClick.AddListener(StartGame);
        endGameButton.onClick.AddListener(Application.Quit);
        eventSystem = EventSystem.current;
        EventSystem.current.sendNavigationEvents = true;
        EventSystem.current.SetSelectedGameObject(startGameButton.gameObject);
        SetFadeAnimState(false);
        SetFadeAnimState(false, null, endingViewCanvasGroup);
        SetFadeAnimState(true, () => { eventSystem.gameObject.SetActive(true); }, mainTitleCanvasGroup);
    }

    private void StartGame()
    {
        if (gameStarted)
            return;

        gameStarted = true;
        eventSystem.gameObject.SetActive(false);
        SetFadeAnimState(false, null, mainTitleCanvasGroup);
        SetFadeAnimState(true, () => { LightManThiefSingleton.InitGame(this); });
    }

    private void OnButtonInputDetected(InputAction.CallbackContext context)
    {
        if (gameStarted && endingViewCanvasGroup.interactable)
            Application.Quit();
        else if (!gameStarted && mainTitleCanvasGroup.interactable)
            StartGame();
    }

    #endregion
}
