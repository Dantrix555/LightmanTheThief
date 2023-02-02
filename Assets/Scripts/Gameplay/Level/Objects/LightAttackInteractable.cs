using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightAttackInteractable : Interactable
{
    #region Fields and properties

    [Header("Light Attack settings")]
    [SerializeField] [Range(1f, 2f)]
    private float lightAttackTime;

    public float LightAttackTime => lightAttackTime;

    #endregion

    #region Interactable Inheritance

    public override void OnInteractorDetected(Interactor interactor)
    {
        if(interactor is EnemyController)
        {
            EnemyController enemyReference = interactor as EnemyController;
            enemyReference.SetCharacterDefeated();
        }
    }

    public override void SetupInteractable(object extraParam = null)
    {
        
    }

    #endregion
}
