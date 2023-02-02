using System;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Controls general UI behaviour
/// </summary>
public class InGameUIController : MonoBehaviour, IObserver
{
    #region Fields and properties

    [Header("Main panels")]
    [SerializeField]
    private HUDManager hudManager;
    [SerializeField]
    private ScoreUIPanel scorePanel;
    [SerializeField]
    private BribeHUD bribeDialogPanel;

    public ScoreUIPanel ScorePanel => scorePanel;
    
    private List<Item> cachedItemsData;

    #endregion

    #region IObserver implementation

    public void UpdateSubjectState(ISubject subject)
    {
        if (subject is Item)
        {
            Item subjectItem = subject as Item;
            cachedItemsData.Add(subjectItem);

            switch(subjectItem.ItemData.GameItemType)
            {
                case GameItems.Gem:
                    hudManager.SetImageItemActiveState(subjectItem.ItemData, true);
                    break;

                case GameItems.FlashBomb:
                    hudManager.UpdateFlashBombValue(true);
                    break;

                default:
                    break;
            }
        }
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Setup UI panels
    /// </summary>
    /// <param name="levelData">Level data reference</param>
    public void InitController(LevelData levelData)
    {
        hudManager.SetupPanel(levelData);
        scorePanel.SetupPanel(levelData);
        bribeDialogPanel.SetupPanel(levelData);
        cachedItemsData = new List<Item>();
    }

    /// <summary>
    /// Update flashbomb amount view after used
    /// </summary>
    public void OnUsedFlashBomb() => hudManager.UpdateFlashBombValue(false);

    /// <summary>
    /// Set button new enable state
    /// </summary>
    /// <param name="buttonToEnable">Button enum reference</param>
    /// <param name="enableState">New button enable state</param>
    public void SetEnableStateToAttackButton(ButtonAttack buttonToEnable, bool enableState)
    {
        HoverableUIButton attackButton = buttonToEnable == ButtonAttack.FlashBomb ? hudManager.FlashBombAttackButton : hudManager.BribeAttackButton;
        attackButton.SetButtonEnableState(enableState);
    }

    public void SetBribeUIActive(DialogScriptableObject bribeDialog, Action onEnemyAceptedBribe, Action onEnemyCanceledBribe)
    {
        onEnemyAceptedBribe += UpdateMoneyValue;
        onEnemyCanceledBribe += UpdateMoneyValue;
        bribeDialogPanel.AfterDialogUpdate(onEnemyAceptedBribe, onEnemyCanceledBribe);
        bribeDialogPanel.OpenPanel(bribeDialog);
    }

    public bool ScorePanelClosed() => scorePanel.PanelClosed;

    #endregion

    #region Private Methods

    private void UpdateMoneyValue() => hudManager.UpdateMoneyAmount();

    #endregion
}
