using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Base Item class that contains basic and subject behaviours
/// </summary>
public abstract class Item : Interactable, ISubject
{
    #region Fields and Properties

    [Header("Item base data")]
    [SerializeField]
    private ItemData itemData;

    protected Vector3 startingItemPosition;
    
    private List<IObserver> allowedObservers;

    public List<IObserver> AllowedObservers => allowedObservers;
    public ItemData ItemData => itemData;

    #endregion

    #region ISubject Implementation

    public void Notify()
    {
        OnItemGet();

        foreach (IObserver observers in allowedObservers)
        {
            observers.UpdateSubjectState(this);
        }
    }

    public void Subscribe(IObserver observer)
    {
        allowedObservers.Add(observer);
    }

    public void UnSuscribe(IObserver observer)
    {
        allowedObservers.Remove(observer);
    }

    #endregion

    #region Interactable inheritance

    public override void SetupInteractable(object extraParam = null)
    {
        allowedObservers = new List<IObserver>();
        startingItemPosition = transform.position;
        SetupItem();
    }

    public override void OnInteractorDetected(Interactor interactor)
    {
        if (interactor is PlayerController)
            Notify();
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Setup item with its basic data
    /// </summary>
    protected abstract void SetupItem();

    /// <summary>
    /// Action when a player takes this gem
    /// </summary>
    protected abstract void OnItemGet();

    /// <summary>
    /// Action when player failed mission getting this gem
    /// </summary>
    public abstract void OnItemLost();

    #endregion
}
