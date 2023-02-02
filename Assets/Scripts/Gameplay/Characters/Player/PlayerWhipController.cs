using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWhipController : Interactable
{
    #region Fields and properties

    [Header("Whip use values")]
    [SerializeField] [Range(0.3f, 1f)]
    private float whipSpawnTime;

    private Transform playerTransform;

    #endregion

    #region Interactable implementation

    public override void OnInteractorDetected(Interactor interactor)
    {

        if(interactor is EnemyController && Vector3.Distance(interactor.transform.position, transform.position) < 1.5f)
        {
            EnemyController enemyDetected = interactor as EnemyController;
            enemyDetected.OnEnemyBribe(playerTransform);
        }
    }

    public override void SetupInteractable(object extraParam = null)
    {
        gameObject.SetActive(false);
    }

    #endregion

    #region Public Methods

    public void UseWhip(Transform playerTransform)
    {
        if(!gameObject.activeInHierarchy)
        {
            gameObject.SetActive(true);
            this.playerTransform = playerTransform;
            StartCoroutine(SetWhipActiveTime());
        }
    }

    #endregion

    #region Private Methods

    private IEnumerator SetWhipActiveTime()
    {
        yield return new WaitForSeconds(whipSpawnTime);
        gameObject.SetActive(false);
    }

    #endregion
}
