using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

/// <summary>
/// Controls UI Buttons looks according if it's enabled, disabled, being pressed or not
/// </summary>
public class HoverableUIButton : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
{
    #region Fields and properties

    [SerializeField]
    private List<Image> hoverableButtonImages;

    [SerializeField]
    private Color enabledButtonColor;
    [SerializeField]
    private Color disabledButtonColor;

    #endregion

    #region Unity Methods

    private void Start()
    {
        SetColorToHoverableButtonImages(enabledButtonColor);
    }

    #endregion

    #region Drag interfaces implementation

    public void OnBeginDrag(PointerEventData eventData)
    {
        SetColorToHoverableButtonImages(disabledButtonColor);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        SetColorToHoverableButtonImages(enabledButtonColor);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SetColorToHoverableButtonImages(disabledButtonColor);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        SetColorToHoverableButtonImages(enabledButtonColor);
    }

    #endregion

    #region Public Methods

    public void SetButtonEnableState(bool isEnable)
    {
        gameObject.SetActive(isEnable);
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Set an specific color to UI Buttons images
    /// </summary>
    /// <param name="newColor">New color to set to the buttons</param>
    private void SetColorToHoverableButtonImages(Color newColor)
    {
        foreach (Image image in hoverableButtonImages)
        {
            image.color = newColor;
        }
    }

    #endregion
}
