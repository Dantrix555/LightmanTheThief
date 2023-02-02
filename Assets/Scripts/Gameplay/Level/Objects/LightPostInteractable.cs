using System;
using System.Collections;
using UnityEngine;

public class LightPostInteractable : Interactable
{
    #region Fields and properties

    [Header("Lightpost detection data")]
    [SerializeField] [Range(1f, 3f)] [Min(1f)]
    private float detectionRadius;

    private PlayerController playerControllerReference;
    private Coroutine onDetectedPlayer;
    private Action<bool> onNearLightUpdate;

    #endregion

    #region Interactable Inheritance

    public override void OnInteractorDetected(Interactor interactor)
    {
        if (interactor is PlayerController && playerControllerReference == null)
        {
            playerControllerReference = interactor as PlayerController;
            
            if (onDetectedPlayer != null)
                StopCoroutine(onDetectedPlayer);

            onDetectedPlayer = StartCoroutine(PlayerDetectedCoroutine());
        }
    }

    public override void SetupInteractable(object extraParam = null)
    {
        
    }

    #endregion

    #region Public Methods

    public void SetupLightPost(Action<bool> onNearLightUpdate)
    {
        this.onNearLightUpdate = onNearLightUpdate;
    }

    #endregion

    #region Private Methods

    private IEnumerator PlayerDetectedCoroutine()
    {
        onNearLightUpdate?.Invoke(true);
        playerControllerReference.NearLightPostPosition = transform.position;
        yield return new WaitUntil(() => (((int)Vector3.Distance(transform.position, playerControllerReference.transform.position)) > detectionRadius));
        onNearLightUpdate?.Invoke(false);
        playerControllerReference = null;
    }

    #endregion
}
