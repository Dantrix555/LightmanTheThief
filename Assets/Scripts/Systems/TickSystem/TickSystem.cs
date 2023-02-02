using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Static tick system used for triggering events every tick
/// </summary>
public class TickSystem : MonoBehaviour
{

    #region Fields and properties

    private static Action OnTickEvent;

    private static int TICK_DELAY_IN_SECONDS = 1;

    private static bool tickStarted = false;

    private static List<Action> cachedOnTickActionList;

    private Coroutine tickCoroutine;

    #endregion

    private TickSystem() { }

    #region Unity Methods

    private void Awake()
    {
        cachedOnTickActionList = new List<Action>();
        StartTickingSystem();
    }

    private void OnApplicationQuit()
    {
        StopTicker();
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Add new action to be called every tick
    /// </summary>
    /// <param name="newTickingAction">New action to add</param>
    public static void AddNewTickingAction(Action newTickingAction)
    {
        cachedOnTickActionList.Add(newTickingAction);
        OnTickEvent += newTickingAction;
    }

    /// <summary>
    /// Remove an action from being called every tick
    /// </summary>
    /// <param name="tickingActionToRemove">Action to be removed</param>
    public static void RemoveTickingAction(Action tickingActionToRemove)
    {
        cachedOnTickActionList.Remove(tickingActionToRemove);
        OnTickEvent -= tickingActionToRemove;
    }

    /// <summary>
    /// Start ticking system
    /// </summary>
    public void StartTickingSystem()
    {
        if (tickStarted)
            return;

        tickStarted = true;

        if (tickCoroutine != null)
        {
            StopCoroutine(tickCoroutine);
        }
        tickCoroutine = StartCoroutine(SetTickingTask());
    }

    /// <summary>
    /// Stop ticking system
    /// </summary>
    public void StopTicker()
    {
        tickStarted = false;
        foreach (Action action in cachedOnTickActionList)
        {
            OnTickEvent -= action;
        }
        StopCoroutine(SetTickingTask());

        cachedOnTickActionList.Clear();

    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Init ticking task and execute events every set delay milliseconds
    /// </summary>
    private IEnumerator SetTickingTask()
    {
        while(tickStarted)
        {
            yield return new WaitForSeconds(TICK_DELAY_IN_SECONDS);
            OnTickEvent?.Invoke();
        }
    }

    #endregion

}
