using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base interactor object class
/// </summary>
public class Interactor : MonoBehaviour
{
    #region Fields and properties

    protected const float BASE_DETECTION_RADIUS = 1f;
    protected const float BASE_LINEAL_DETECTION_DISTANCE = 3f; 

    [SerializeField]
    private LayerMask interactableLayer;

    [HideInInspector]
    protected float detectionRadius = BASE_DETECTION_RADIUS;
    [HideInInspector]
    protected float detectionLinearDistance = BASE_LINEAL_DETECTION_DISTANCE;

    #endregion

    #region Public Methods

    /// <summary>
    /// Check detected colliders around and select those ones to interact with them
    /// </summary>
    public void DetectInteractablesAround()
    {
        Collider2D[] collidersDetected = Physics2D.OverlapCircleAll(transform.position, detectionRadius, interactableLayer);

        foreach (Collider2D item in collidersDetected)
        {
            if(item.TryGetComponent(out Interactable newInteractable))
            {
                newInteractable.OnInteractorDetected(this);
            }
        }
    }

    /// <summary>
    /// Check detected colliders in a box area and select those ones to interact with them
    /// </summary>
    public void DetectInteractablesInFront(Vector3 raycastDirection)
    {
        Vector3 boxColliderSize = Mathf.Abs(raycastDirection.x) > Mathf.Abs(raycastDirection.y) ? new Vector3(detectionLinearDistance, detectionLinearDistance) : new Vector3(detectionLinearDistance, detectionLinearDistance);
        Collider2D[] collidersDetected = Physics2D.OverlapBoxAll(transform.position + raycastDirection.normalized, boxColliderSize, interactableLayer);

        foreach (Collider2D item in collidersDetected)
        {
            if (item.TryGetComponent(out Interactable newInteractable))
            {
                newInteractable.OnInteractorDetected(this);
            }
        }
    }

    #endregion
}
