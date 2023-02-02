using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatUIView : MonoBehaviour
{
    #region Fields and properties

    [Header("Stat components")]
    [SerializeField]
    private Image statImage;
    [SerializeField]
    private TextMeshProUGUI statTextValue;

    #endregion

    #region Public Methods

    /// <summary>
    /// Set new sprite for image stat
    /// </summary>
    /// <param name="imageNewSprite">New sprite to be set</param>
    public void SetStatImage(Sprite imageNewSprite)
    {
        if (statImage == null)
        {
            Debug.LogError($"There's no image attached to stat game object {gameObject.name}");
            return;
        }

        statImage.sprite = imageNewSprite;
    }

    /// <summary>
    /// Set new text with adquired money
    /// </summary>
    /// <param name="statMoneyValue">Money value to add</param>
    public void SetTextValue(string statMoneyValue)
    {
        if (statTextValue == null)
        {
            Debug.LogError($"There's no TMP attached to stat game object {gameObject.name}");
            return;
        }

        statTextValue.text = statMoneyValue;
    }

    #endregion
}
