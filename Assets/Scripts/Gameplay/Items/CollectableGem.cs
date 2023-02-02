using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Collectable gem item controller
/// </summary>
public class CollectableGem : Item
{
    #region Fields and properties

    [Header("Item Components")]
    [SerializeField]
    private SpriteRenderer itemSpriteRenderer;

    #endregion

    #region IItem implementation

    protected override void SetupItem()
    {
        itemSpriteRenderer.sprite = ItemData.ItemImage;
    }

    protected override void OnItemGet()
    {
        gameObject.SetActive(false);
    }

    public override void OnItemLost()
    {
        transform.position = startingItemPosition;
        gameObject.SetActive(true);
    }

    #endregion
}
