using System;
using UnityEngine;
using Pathfinding;
using System.Collections.Generic;

/// <summary>
/// Controls main level behaviour and store it's important data
/// </summary>
public class LevelController : MonoBehaviour
{
    #region Fields and properties

    [SerializeField]
    private ItemsController levelItems;
    [SerializeField]
    private EnemiesManager levelEnemies;
    [SerializeField]
    private LightPostsManager levelLights;
    [SerializeField]
    private AstarPath path;
    [SerializeField]
    private PolygonCollider2D boundaryCollider;

    public List<ItemData> LevelItemsData => levelItems.GetItemDataList();
    public PolygonCollider2D LevelBoundary => boundaryCollider;

    #endregion

    #region Public Methods

    public void OnStartLevel(List<IObserver> itemsObserver, Action<bool> onNearLightUpdate)
    {
        levelItems.InitController(itemsObserver);
        levelEnemies.SetNewEnemiesActiveState(true);
        levelLights.SetupLightPosts(onNearLightUpdate);
        path.Scan(path.graphs);
    }

    public void OnRestartLevel()
    {
        levelItems.DropPlayerItems();
        levelEnemies.SetNewEnemiesActiveState(true);
        levelEnemies.StopEnemiesChasingPlayer();
    }

    public void OnFinishLevel()
    {
        levelEnemies.SetNewEnemiesActiveState(false);
        levelEnemies.StopEnemiesChasingPlayer();
        Destroy(gameObject);
    }

    public void SetNewEnemiesAlertState(bool state)
    {
        levelEnemies.SetEnemyAlertedSpeedState(state);
    }

    public void ActivateLightAttack(Vector3 attackPosition, Action reactivateFlashBombUse)
    {
        levelLights.InstanceLightAttackOnPosition(attackPosition, reactivateFlashBombUse);
    }

    #endregion
}
