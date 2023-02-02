using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPostsManager : MonoBehaviour
{
    #region Fields and properties

    [Header("Main lights references")]
    [SerializeField]
    private List<LightPostInteractable> levelLights;
    [SerializeField]
    private LightAttackInteractable lightAttackInteractable;

    private Action reactivateFlashBombUse;

    #endregion

    #region Public Methods

    public void SetupLightPosts(Action<bool> onNearLightUpdate)
    {
        lightAttackInteractable.gameObject.SetActive(false);
        foreach (LightPostInteractable light in levelLights)
        {
            light.SetupLightPost(onNearLightUpdate);
        }
    }

    public void InstanceLightAttackOnPosition(Vector3 lightAttackPosition, Action reactivateFlashBombUse)
    {
        lightAttackInteractable.transform.position = lightAttackPosition;
        this.reactivateFlashBombUse = reactivateFlashBombUse;
        lightAttackInteractable.gameObject.SetActive(true);
        StartCoroutine(DeactivateLightAttack());
    }

    #endregion

    #region Private Methods

    private IEnumerator DeactivateLightAttack()
    {
        yield return new WaitForSeconds(lightAttackInteractable.LightAttackTime);
        lightAttackInteractable.gameObject.SetActive(false);
        reactivateFlashBombUse?.Invoke();
    }

    #endregion

}
