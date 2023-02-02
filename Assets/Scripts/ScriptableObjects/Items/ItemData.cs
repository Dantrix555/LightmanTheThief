using UnityEngine;
using UnityEngine.UI;

public enum GameItems { Gem, FlashBomb }

/// <summary>
/// Game item data
/// </summary>
[CreateAssetMenu(fileName ="New Item Data", menuName = "Lightman Thief Data/New Item Data")]
public class ItemData : ScriptableObject
{
    #region Fields and properties

    [SerializeField]
    private GameItems gameItemType;
    [SerializeField]
    private Sprite itemImage;
    [SerializeField]
    [Range(10, 100)]
    private int itemValue;

    public GameItems GameItemType => gameItemType;
    public Sprite ItemImage => itemImage;
    public int ItemValue => itemValue;

    #endregion
}
