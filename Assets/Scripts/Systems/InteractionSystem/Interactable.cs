using UnityEngine;
using System;
using System.Collections.Generic;

public abstract class Interactable : MonoBehaviour
{
    protected List<Interactor> allowedInteractors;

    public abstract void SetupInteractable(object extraParam = null);

    public abstract void OnInteractorDetected(Interactor interactor);
}
