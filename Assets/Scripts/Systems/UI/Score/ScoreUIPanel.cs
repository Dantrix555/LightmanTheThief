using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

/// <summary>
/// Control behaviour of stat panel
/// </summary>
public class ScoreUIPanel : MonoBehaviour, IPanel
{
    #region Fields and properties

    [Header("Stats values")]
    [SerializeField]
    private StatUIView clockStatView;
    [SerializeField]
    private StatUIView flashBombStatView;
    [SerializeField]
    private StatUIView totalMoneyStatView;
    [SerializeField]
    private StatUIView[] itemsStatViews;

    private LevelData cachedLevelData;
    private MainGameInputs mainGameInputs;

    private bool panelClosed;
    public bool PanelClosed => panelClosed;

    private MainGameInputs uiInputs;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        uiInputs = new MainGameInputs();
        uiInputs.UiMap.Accept.performed += OnAcceptScore;
        uiInputs.UiMap.Cancel.performed += OnAcceptScore;
    }

    private void OnEnable()
    {
        uiInputs.Enable();
    }

    private void OnDisable()
    {
        uiInputs.Disable();
    }


    #endregion

    #region IPanel implementation

    public void SetupPanel(LevelData actualLevelData)
    {
        cachedLevelData = actualLevelData;
        
        List<ItemData> actualItemsList = cachedLevelData.LevelItemsData;
        for(int i = 0; i < actualItemsList.Count; i++)
        {
            if (i > itemsStatViews.Length - 1)
                break;

            itemsStatViews[i].SetStatImage(actualItemsList[i].ItemImage);
            itemsStatViews[i].SetTextValue($" $0");
            itemsStatViews[i].gameObject.SetActive(true);
        }

        mainGameInputs = new MainGameInputs();
        mainGameInputs.UiMap.Accept.performed += OnAcceptScore;

        ClosePanel();
    }

    public void OpenPanel(object aditionalOpenData = null)
    {
        SetStatsMoneyValues();
        panelClosed = false;
        gameObject.SetActive(true);
    }

    public void ClosePanel()
    {
        panelClosed = true;
        gameObject.SetActive(false);
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Set money gained at the end of the level
    /// </summary>
    public void SetStatsMoneyValues()
    {
        int remainingTimeValue = cachedLevelData.MoneyPerSecondRemaining * cachedLevelData.RemainingTime;
        int remainingFlashBombsValue = cachedLevelData.PlayerStatsData.FlashShots * cachedLevelData.PlayerStatsData.FlashShotsMoneyValue;
        int totalMoneyGained = remainingTimeValue + remainingFlashBombsValue;

        clockStatView.SetTextValue($" ${remainingTimeValue}");
        flashBombStatView.SetTextValue($" ${remainingFlashBombsValue}");

        for(int i = 0; i < cachedLevelData.LevelItemsData.Count; i++)
        {
            for(int j = 0; j < cachedLevelData.PlayerStatsData.ItemsCollected.Count; j++)
            {
                if(cachedLevelData.PlayerStatsData.ItemsCollected[j] == cachedLevelData.LevelItemsData[i])
                {
                    int itemValue = cachedLevelData.LevelItemsData[j].ItemValue;
                    itemsStatViews[i].SetTextValue($" ${itemValue}");
                    totalMoneyGained += itemValue;
                }
            }
        }

        totalMoneyStatView.SetTextValue($" ${totalMoneyGained}");
        cachedLevelData.PlayerStatsData.UpdatePlayerMoneyValue(totalMoneyGained);
    }

    #endregion

    #region Private Methods

    private void OnAcceptScore(InputAction.CallbackContext context)
    {
        ClosePanel();
    }

    #endregion
}
