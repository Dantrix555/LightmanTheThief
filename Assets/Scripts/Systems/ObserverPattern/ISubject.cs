using System.Collections.Generic;

/// <summary>
/// Generic subject interface
/// </summary>
public interface ISubject
{
    /// <summary>
    /// List of allowed observer to act according to this subject
    /// </summary>
    public List<IObserver> AllowedObservers { get; }

    /// <summary>
    /// Set a new observer to this subject
    /// </summary>
    /// <param name="observer">New Observer</param>
    public void Subscribe(IObserver observer);

    /// <summary>
    /// Remove an specific observer from this object
    /// </summary>
    /// <param name="observer">Observer to remove</param>
    public void UnSuscribe(IObserver observer);

    /// <summary>
    /// Notification on some specific behaviour of this subject
    /// </summary>
    public void Notify();
}
