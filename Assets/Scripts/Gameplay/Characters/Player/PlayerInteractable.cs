using UnityEngine;

/// <summary>
/// Controls player as interactable object behaviours
/// </summary>
public class PlayerInteractable : Interactable
{
    #region Fields and properties

    [SerializeField] [Range(1f, 3f)]
    private float maxEnemyNearAllowedDistance;

    public bool playerIsFlashing;

    #endregion

    #region Interactable inheritance

    public override void OnInteractorDetected(Interactor interactor)
    {
        if (interactor is EnemyController)
            CheckEnemyCloseness(interactor as EnemyController);
    }

    public override void SetupInteractable(object extraParam = null)
    {

    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Check how close is enemy from player, if it's really close, reset level, otherwise set player detected alarm
    /// </summary>
    /// <param name="enemy"></param>
    private void CheckEnemyCloseness(EnemyController enemy)
    {
        bool enemyIsVeryNear = Vector3.Distance(enemy.transform.position, transform.position) < maxEnemyNearAllowedDistance ? true : false;
        if (enemyIsVeryNear)
            InGameController.Instance.onGameOverAction?.Invoke();
        else
            enemy.SetPlayerAlertState(true, transform);
    }

    #endregion
}
