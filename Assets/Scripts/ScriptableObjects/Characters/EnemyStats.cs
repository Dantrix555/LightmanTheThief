using UnityEngine;

/// <summary>
/// Enemy main stats data
/// </summary>
[CreateAssetMenu(fileName = "New Enemy Custom Stats", menuName = "Lightman Thief Data/New Enemy Stats")]
public class EnemyStats : CharacterStats
{
    #region Fields and properties

    [Header("Logic and mechanics related data")]
    [SerializeField]
    [Range(0, 100)]
    private int moneyToBribe;
    [SerializeField]
    private Color lanternColor;

    public int MoneyToBribe => moneyToBribe;
    public Color LanternColor => lanternColor;

    #endregion
}
