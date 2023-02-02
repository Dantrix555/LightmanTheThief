using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls global light base behaviour
/// </summary>
public class GlobalLightController : MonoBehaviour
{
    #region Fields and properties

    [SerializeField]
    private Animator globalLightAnimator;

    #endregion

    #region Unity Methods

    public void Awake()
    {
        SetLightAnimationState(false);
    }

    #endregion

    #region Public Methods

    public void SetLightAnimationState(bool setPlayerDetectedLight)
    {
        globalLightAnimator.SetBool("PlayerDetected", setPlayerDetectedLight);
    }

    #endregion

}
