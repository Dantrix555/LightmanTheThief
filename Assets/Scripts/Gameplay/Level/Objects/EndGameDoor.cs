using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameDoor : Interactable
{
    #region Fields and properties

    private bool playerIsInDoor = false;

    #endregion

    #region Item Inheritance

    public override void OnInteractorDetected(Interactor interactor)
    {
        if(interactor is PlayerController && !playerIsInDoor)
        {
            playerIsInDoor = true;
            InGameController.Instance.onLevelFinishedAction?.Invoke();
        }
    }

    public override void SetupInteractable(object extraParam = null)
    {
        
    }

    #endregion
}
