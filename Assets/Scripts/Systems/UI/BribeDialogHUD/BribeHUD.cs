using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BribeHUD : MonoBehaviour, IPanel
{
    #region Fields and properties

    [Header("Main Bribe HUD Components")]
    [SerializeField]
    private Image actualCharacterUIImage;
    [SerializeField]
    private Button acceptButton;
    [SerializeField]
    private Button cancelButton;
    [SerializeField]
    private TextMeshProUGUI actualDialogText;

    private int actualDialogLineIndex;
    private int actualDialogLineLength;
    private DialogScriptableObject actualDialogLinesScriptableObject;
    private DialogLine actualDialogLine;

    private bool lastResponseWasNegative;
    private Action onEnemyAceptedBribe;
    private Action onEnemyCanceledBribe;

    private bool closeBeforeEndingLines;
    private LevelData cachedLevelData;
    private MainGameInputs gameInputs;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        gameInputs = new MainGameInputs();
        gameInputs.UiMap.Accept.performed += (contect) => { OnAccept(); };
        gameInputs.UiMap.Cancel.performed += (contect) => { OnCancel(); };
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

    #region IPanel implementation

    public void SetupPanel(LevelData actualLevelData)
    {
        acceptButton.onClick.AddListener(OnAccept);
        cancelButton.onClick.AddListener(OnCancel);
        cachedLevelData = actualLevelData;
        gameObject.SetActive(false);
    }

    public void OpenPanel(object aditionalOpenData = null)
    {
        if (gameObject.activeInHierarchy)
            return;

        actualDialogLinesScriptableObject = (DialogScriptableObject)aditionalOpenData;
        actualDialogLineLength = actualDialogLinesScriptableObject.GetLinesLength();
        actualDialogLineIndex = -1;
        closeBeforeEndingLines = false;
        lastResponseWasNegative = false;
        SetNextDialogLines();
        gameObject.SetActive(true);
    }

    public void ClosePanel()
    {
        int moneyToLose = actualDialogLinesScriptableObject.BribeMoney <= 0 ? 0 : cachedLevelData.PlayerStatsData.PlayerMoney < actualDialogLinesScriptableObject.BribeMoney ? 
            cachedLevelData.PlayerStatsData.PlayerMoney : actualDialogLinesScriptableObject.BribeMoney;

        cachedLevelData.PlayerStatsData.UpdatePlayerMoneyValue(-moneyToLose);

        if (lastResponseWasNegative || cachedLevelData.PlayerStatsData.PlayerMoney < actualDialogLinesScriptableObject.BribeMoney || actualDialogLinesScriptableObject.BribeMoney <= 0
            || actualDialogLinesScriptableObject.DialogIsATrap)
        {
            onEnemyCanceledBribe?.Invoke();
            Debug.LogError("Bribe failed");
        }
        else
        {
            onEnemyAceptedBribe?.Invoke();
            Debug.LogError("Bribe worked");
        }

        LightManThiefSingleton.SetDialogGameState(false);

        actualDialogLinesScriptableObject = null;
        actualDialogLine = new DialogLine();
        actualDialogLineLength = 0;
        actualDialogLineIndex = -1;

        gameObject.SetActive(false);
    }

    #endregion

    #region Public Methods

    public void AfterDialogUpdate(Action onEnemyAceptedBribe, Action onEnemyCanceledBribe)
    {
        this.onEnemyAceptedBribe = onEnemyAceptedBribe;
        this.onEnemyCanceledBribe = onEnemyCanceledBribe;
    }

    #endregion

    #region Private Methods

    private void SetNextDialogLines(bool hasNegativeResponse = false)
    {
        if (actualDialogLineIndex >= actualDialogLineLength - 1 || closeBeforeEndingLines)
        {
            ClosePanel();
            return;
        }

        if (actualDialogLineIndex >= 0 && actualDialogLine.LineHasResponses() && !actualDialogLine.IsACharacterDialog)
        {
            actualDialogText.text = hasNegativeResponse ? actualDialogLine.NegativeDialogResponse : actualDialogLine.AffirmativeDialogRespone;
            cancelButton.transform.parent.gameObject.SetActive(false);
            //dialogHasBeenResponded = true;
            lastResponseWasNegative = hasNegativeResponse;
            closeBeforeEndingLines = actualDialogLine.closeOnAffirmative && !hasNegativeResponse;
            return;
        }

        actualDialogLineIndex++;
        actualDialogLine = actualDialogLinesScriptableObject.DialogLines[actualDialogLineIndex];
        actualDialogText.text = actualDialogLine.LineDialog;
        actualCharacterUIImage.sprite = actualDialogLine.CharacterDialogSprite;
        cancelButton.transform.parent.gameObject.SetActive(actualDialogLine.LineHasResponses());
    }

    private void OnAccept()
    {  
        SetNextDialogLines();
    }

    private void OnCancel()
    {
        SetNextDialogLines(true);
    }

    #endregion
}
