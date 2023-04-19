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
    protected float scanRadius = BASE_DETECTION_RADIUS;
    [HideInInspector]
    protected float detectionLinearDistance = BASE_LINEAL_DETECTION_DISTANCE;

    private Collider2D[] collidersAroundDetectedList = new Collider2D[10];
    private Collider2D[] collidersInFrontDetectedList = new Collider2D[10];
    private int collidersAroundDetected;
    private int collidersInFrontDetected;

    #endregion

    #region Public Methods

    /// <summary>
    /// Check detected colliders around and select those ones to interact with them
    /// </summary>
    public void DetectInteractablesAround()
    {
        collidersAroundDetected = Physics2D.OverlapCircleNonAlloc(transform.position, scanRadius, collidersAroundDetectedList, interactableLayer);

        if (collidersAroundDetected <= 0)
            return;

        for(int i = 0; i < collidersAroundDetected; i++)
        {
            if(collidersAroundDetectedList[i].gameObject != gameObject)
            {
                if (collidersAroundDetectedList[i].TryGetComponent(out Interactable newInteractable))
                {
                    newInteractable.OnInteractorDetected(this);
                }
            }
        }
    }

    /// <summary>
    /// Check detected colliders in a box area and select those ones to interact with them
    /// </summary>
    public void DetectInteractablesInFront(Vector3 raycastDirection)
    {
        Vector3 boxColliderSize = Mathf.Abs(raycastDirection.x) > Mathf.Abs(raycastDirection.y) ? new Vector3(detectionLinearDistance, detectionLinearDistance) : new Vector3(detectionLinearDistance, detectionLinearDistance);
        collidersInFrontDetected = Physics2D.OverlapBoxNonAlloc(transform.position + raycastDirection.normalized, boxColliderSize, 0, collidersInFrontDetectedList, interactableLayer);

        if (collidersInFrontDetected <= 0)
            return;

        for (int i = 0; i < collidersInFrontDetected; i++)
        {
            if (collidersInFrontDetectedList[i].gameObject != gameObject)
            {
                if (collidersInFrontDetectedList[i].TryGetComponent(out Interactable newInteractable))
                {
                    newInteractable.OnInteractorDetected(this);
                }
            }
        }
    }

    #endregion
}
