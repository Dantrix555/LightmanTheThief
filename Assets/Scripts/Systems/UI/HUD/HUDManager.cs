using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum ButtonAttack { FlashBomb, Bribe }

/// <summary>
/// Main HUD Manager that stores and update in game data
/// </summary>
public class HUDManager : MonoBehaviour, IPanel
{
    #region Fields and properties

    [Header("Digital buttons references")]
    [SerializeField]
    private HoverableUIButton flashBombAttackButton;
    [SerializeField]
    private HoverableUIButton bribeAttackButton;

    [Space(5)]
    [Header("Main Text Elements")]
    [SerializeField]
    private TextMeshProUGUI moneyAmountText;
    [SerializeField]
    private TextMeshProUGUI flashBombText;
    [SerializeField]
    private TextMeshProUGUI timerText;

    [Space(5)]
    [Header("Items images references")]
    [SerializeField]
    private Image[] itemsImages;

    private LevelData cachedLevelData;

    public HoverableUIButton FlashBombAttackButton => flashBombAttackButton;
    public HoverableUIButton BribeAttackButton => bribeAttackButton;

    #endregion

    #region IPanel implementation

    public void SetupPanel(LevelData actualLevelData)
    {
        cachedLevelData = actualLevelData;

        for (int i = 0; i < itemsImages.Length; i++)
        {
            itemsImages[i].gameObject.SetActive(cachedLevelData.LevelItemsData[i] != null);
            if (cachedLevelData.LevelItemsData[i] != null)
            {
                itemsImages[i].sprite = cachedLevelData.LevelItemsData[i].ItemImage;
                SetImageItemActiveState(cachedLevelData.LevelItemsData[i], false);
            }
        }

        TickSystem.AddNewTickingAction(UpdateTimer);
        flashBombAttackButton.SetButtonEnableState(false);
        bribeAttackButton.SetButtonEnableState(true);
        OpenPanel();
    }

    public void OpenPanel(object aditionalOpenData = null)
    {
        cachedLevelData.RemainingTime = cachedLevelData.TimeToScapeInSeconds;

        flashBombText.text = $"X {cachedLevelData.PlayerStatsData.FlashShots}";
        UpdateMoneyAmount();
        gameObject.SetActive(true);
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Update remaining timer value in HUD
    /// </summary>
    private void UpdateTimer()
    {
        cachedLevelData.RemainingTime--;
        timerText.text = cachedLevelData.RemainingTime.ToString();

        if (cachedLevelData.RemainingTime <= 0)
        {
            TickSystem.RemoveTickingAction(UpdateTimer);
            InGameController.Instance.onGameOverAction?.Invoke();
        }
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Set item image a new active state
    /// </summary>
    /// <param name="item">Item image data</param>
    /// <param name="state">New state for image</param>
    public void SetImageItemActiveState(ItemData item, bool state)
    {
        int itemColor = state ? 1 : 0;
        for (int i = 0; i < itemsImages.Length; i++)
        {
            if (itemsImages[i].sprite == item.ItemImage)
            {
                itemsImages[i].color = new Color(itemColor, itemColor, itemColor, 1);

                if(state)
                    cachedLevelData.PlayerStatsData.ItemsCollected.Add(item);
            }
        }
    }

    /// <summary>
    /// Update flash bomb value when it's gotten by player
    /// </summary>
    public void UpdateFlashBombValue(bool isAddingNewFlashBomb)
    {
        cachedLevelData.PlayerStatsData.UpdateFlashShotsValue(isAddingNewFlashBomb);
        flashBombText.text = $"X {cachedLevelData.PlayerStatsData.FlashShots}";
    }

    public void UpdateMoneyAmount() => moneyAmountText.text = $"X {cachedLevelData.PlayerStatsData.PlayerMoney}";

    #endregion
}
