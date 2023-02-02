using UnityEngine;

/// <summary>
/// Flashbomb item controller
/// </summary>
public class Flashbomb : Item
{
    #region Fields and properties

    [Header("Item Components")]
    [SerializeField]
    private SpriteRenderer itemSpriteRenderer;

    #endregion

    #region Item Inheritance

    protected override void OnItemGet()
    {
        gameObject.SetActive(false);
    }

    public override void OnItemLost()
    {
        transform.position = startingItemPosition;
        gameObject.SetActive(true);
    }

    protected override void SetupItem()
    {
        itemSpriteRenderer.sprite = ItemData.ItemImage;
    }

    #endregion
}
