using UnityEngine;

/// <summary>
/// Base class with characters main behaviours
/// </summary>
public abstract class BaseCharacter : Interactor
{

    #region Fields and properties

    [Header("Character stats")]
    [SerializeField]
    protected CharacterStats characterStatsData;

    [Space(5)]
    [Header("Character Main Components")]
    [SerializeField]
    protected Rigidbody2D characterRigidbody;
    [SerializeField]
    protected Animator characterAnimator;

    [HideInInspector]
    protected float actualCharacterSpeed;

    #endregion

    #region Abstract methods

    /// <summary>
    /// Character setup method, initialize values and behaviours
    /// </summary>
    protected abstract void ConfigureCharacter();

    /// <summary>
    /// Character defeated behaviour method
    /// </summary>
    public abstract void SetCharacterDefeated();

    #endregion

    #region Protected Methods

    /// <summary>
    /// Move character to desired direction
    /// </summary>
    /// <param name="characterDirectionMovement">New direction to move the character</param>
    protected void MoveCharacter(Vector2 characterDirectionMovement)
    {
        characterAnimator.speed = actualCharacterSpeed > characterStatsData.CharacterBaseSpeed ? characterStatsData.RunningSpeedInAnimator : characterStatsData.NormalSpeedInAnimator;
        characterAnimator.SetBool("IsMoving", characterDirectionMovement.x != 0 || characterDirectionMovement.y != 0);

        characterRigidbody.velocity = characterDirectionMovement * actualCharacterSpeed;

        characterAnimator.SetFloat("MoveX", characterDirectionMovement.x);
        characterAnimator.SetFloat("MoveY", characterDirectionMovement.y);

        if (characterDirectionMovement.x != 0 || characterDirectionMovement.y != 0)
        {
            characterAnimator.SetFloat("IdleX", characterDirectionMovement.x);
            characterAnimator.SetFloat("IdleY", characterDirectionMovement.y);
        }
    }

    #endregion

}
