using UnityEngine;

/// <summary>
/// Generic character stats data
/// </summary>
public class CharacterStats : ScriptableObject
{
    #region Fields and properties

    [Header("Physics Data")]
    [SerializeField] [Range(1, 15)]
    private float characterBaseSpeed;
    [SerializeField] [Range(3.5f, 10f)]
    private float characterRunningSpeed;
    [SerializeField] [Range(1f, 5f)] [Min(1f)]
    private float characterDetectionRadius;

    [Space(5)]
    [Header("Animation Data")]
    [SerializeField] [Range(0.5f, 1f)]
    private float normalSpeedInAnimator;
    [SerializeField] [Range(1f, 1.5f)]
    private float runningSpeedInAnimator;

    public float CharacterBaseSpeed => characterBaseSpeed;
    public float CharacterRunningSpeed => characterRunningSpeed;
    public float CharacterDetectionRadius => characterDetectionRadius;
    public float NormalSpeedInAnimator => normalSpeedInAnimator;
    public float RunningSpeedInAnimator => runningSpeedInAnimator;

    #endregion
}
