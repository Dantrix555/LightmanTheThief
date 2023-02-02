using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Player main stats data
/// </summary>
[CreateAssetMenu(fileName = "New Player Custom Stats", menuName = "Lightman Thief Data/New Player Stats")]
public class PlayerStats : CharacterStats
{
    #region Fields and properties

    [Header("Logic and mechanics related data")]
    [SerializeField] [Range(0, 5)]
    private int flashShots;
    [SerializeField] [Range(10, 20)]
    private int flashShotsMoneyValue;
    [SerializeField] [Range(20f, 30f)]
    private float flashDetectionRadius;
    [SerializeField] [Range(1f, 3f)]
    private float flashDuration;
    [SerializeField] [Min(0)]
    private int playerMoney;
    
    private int actualFlashShots;
    private List<ItemData> itemsCollected;

    public int FlashShots => actualFlashShots;
    public int FlashShotsMoneyValue => flashShotsMoneyValue;
    public float FlashDetectionRadius => flashDetectionRadius;
    public float FlashDuration => flashDuration;
    public int PlayerMoney => playerMoney;
    public List<ItemData> ItemsCollected => itemsCollected;

    #endregion

    #region Public Methods

    public void SetStartingPlayerStats()
    {
        itemsCollected = new List<ItemData>();
        actualFlashShots = flashShots;
    }

    public void UpdateFlashShotsValue(bool isIncreasingValue)
    {
        actualFlashShots = isIncreasingValue ? ++actualFlashShots : --actualFlashShots;
    }

    public void UpdatePlayerMoneyValue(int moneyToAddAmount)
    {
        playerMoney += moneyToAddAmount;
        flashShots = actualFlashShots;
    }

    #endregion
}
