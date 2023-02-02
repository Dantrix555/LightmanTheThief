using System;
using UnityEngine;

/// <summary>
/// Scriptable object that store dialog lines
/// </summary>
[CreateAssetMenu(fileName = "New Dialog Scriptable Object", menuName = "Lightman Thief Data/New Dialog Lines")]
public class DialogScriptableObject : ScriptableObject
{
    #region Fields and properties
    
    [SerializeField]
    private bool dialogIsATrap;
    [SerializeField]
    private DialogLine[] dialogLines;

    private int moneyToLose;

    public DialogLine[] DialogLines => dialogLines;
    public bool DialogIsATrap => dialogIsATrap;
    public int BribeMoney { get => moneyToLose; set => moneyToLose = value; }

    #endregion

    #region Public Methods
    
    public int GetLinesLength() => dialogLines.Length;

    #endregion

}

/// <summary>
/// Structure with the main data of a line of dialog
/// </summary>
[Serializable]
public struct DialogLine
{
    #region Fields and properties

    [Header("Dialog components")]
    [SerializeField]
    private bool isCharacterDialog;
    [SerializeField]
    private Sprite characteDialogSprite;

    [Space(5)]
    [Header("Dialog texts")]
    [SerializeField] [TextArea]
    private string dialogLine;
    [SerializeField] [TextArea]
    private string affirmativeResponseDialogLine;
    [SerializeField] [TextArea]
    private string negativeResponseDialogLine;
    [SerializeField]
    public bool closeOnAffirmative;

    public bool IsACharacterDialog => isCharacterDialog;
    public Sprite CharacterDialogSprite => characteDialogSprite;
    
    public string LineDialog => dialogLine;
    public string AffirmativeDialogRespone => affirmativeResponseDialogLine;
    public string NegativeDialogResponse => negativeResponseDialogLine;

    #endregion

    #region Public Methods

    /// <summary>
    /// Check if this line has two responses, negative and positive ones
    /// </summary>
    /// <returns>Return true if has a positive and negative response to the dialog</returns>
    public bool LineHasResponses() => affirmativeResponseDialogLine.Length > 0 && negativeResponseDialogLine.Length > 0;

    #endregion
}