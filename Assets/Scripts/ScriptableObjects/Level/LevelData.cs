using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LevelName
{ 
    None = -1, 
    PersistenceLevel = 0, 
    GameScene = 1 
}

/// <summary>
/// Level principal data
/// </summary>
[CreateAssetMenu(fileName = "New Level Data", menuName = "Lightman Thief Data/New Item Level")]
public class LevelData : ScriptableObject
{
    #region Fields and properties

    [Header("Time related data")]
    [SerializeField] [Range(5, 240)]
    private int timeToScapeInSeconds;
    [SerializeField] [Range(1, 5)]
    private int moneyPerSecondRemaining;

    [Space(5)]
    [Header("Level controllers and player data")]
    [SerializeField]
    private Vector3 playerInitialPosition;
    [SerializeField]
    private PlayerStats playerStatsData;
    [SerializeField]
    private LevelController levelPrefab;

    private List<ItemData> levelItemsData;
    private int secondRemaining;

    public int TimeToScapeInSeconds => timeToScapeInSeconds;
    public int MoneyPerSecondRemaining => moneyPerSecondRemaining;
    public Vector3 PlayerInitialPosition => playerInitialPosition;
    public PlayerStats PlayerStatsData => playerStatsData;
    public LevelController LevelPrefab => levelPrefab;
    public List<ItemData> LevelItemsData => levelItemsData;
    public int RemainingTime { get => secondRemaining; set => secondRemaining = value; }

    #endregion

    #region Public Methods

    /// <summary>
    /// Get a new list of items according to actual level data
    /// </summary>
    /// <param name="actualLevelItems">Items in the actual level</param>
    public void UpdateLevelMainData(List<ItemData> actualLevelItems)
    {
        levelItemsData = new List<ItemData>(actualLevelItems);
    }

    #endregion
}
