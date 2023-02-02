using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    #region Fields and properties

    [SerializeField]
    private List<EnemyController> enemiesInLevel;

    public List<EnemyController> EnemiesInLevel => enemiesInLevel;

    #endregion

    #region Public Methods

    public void SetNewEnemiesActiveState(bool newActiveState)
    {
        foreach (EnemyController enemy in enemiesInLevel)
        {
            enemy.gameObject.SetActive(newActiveState);

            if (newActiveState)
                enemy.ActivateCollider(true);
        }
    }

    public void SetEnemyAlertedSpeedState(bool areEnemiesAlerted)
    {
        foreach (EnemyController enemy in enemiesInLevel)
        {
            if (enemy.isActiveAndEnabled)
                enemy.SetPlayerAlertState(areEnemiesAlerted);
        }
    }

    public void StopEnemiesChasingPlayer()
    {
        foreach (EnemyController enemy in enemiesInLevel)
        {
            if (enemy.isActiveAndEnabled)
            {
                enemy.StopPlayerChasing(true);
            }
        }
    }

    #endregion
}
