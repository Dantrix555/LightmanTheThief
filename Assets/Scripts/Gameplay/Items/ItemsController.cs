using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Controls item setup and store important items data
/// </summary>
public class ItemsController : MonoBehaviour
{
    #region Fields and properties

    [SerializeField]
    private List<Item> levelItems;

    #endregion

    #region Public Methods

    /// <summary>
    /// Setup every item and adds observers to each one
    /// </summary>
    /// <param name="itemsObserver">items observers</param>
    public void InitController(List<IObserver> itemsObserver)
    {
        foreach (Item item in levelItems)
        {
            item.SetupInteractable();
            foreach (IObserver observer in itemsObserver)
            {
                item.Subscribe(observer);
            }
        }
    }

    /// <summary>
    /// Get actual items data list
    /// </summary>
    /// <returns>Items data list</returns>
    public List<ItemData> GetItemDataList()
    {
        List<ItemData> itemDataList = new List<ItemData>();

        foreach (Item item in levelItems)
        {
            itemDataList.Add(item.ItemData);
        }

        return itemDataList;
    }

    /// <summary>
    /// Reset items if player is detected
    /// </summary>
    public void DropPlayerItems()
    {
        foreach (Item item in levelItems)
        {
            item.OnItemLost();
        }
    }

    #endregion
}
