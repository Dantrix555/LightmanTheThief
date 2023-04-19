using UnityEngine;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Experimental.Rendering.Universal;

/// <summary>
/// Controls basic enemy behaviour and movement
/// </summary>
public class EnemyController : BaseCharacter
{
    #region Fields and Properties

    [Header("Enemy base references")]
    [SerializeField]
    private List<Transform> enemyMovePaternPoints;
    [SerializeField]
    private Light2D enemyLanternLight;
    [SerializeField]
    private List<DialogScriptableObject> possibleBribeDialogs;
    [SerializeField]
    private Collider2D enemyCollider;

    [Space(5)]
    [Header("Pathfinding references")]
    [SerializeField]
    private Seeker seeker;
    [SerializeField]
    private LayerMask wallLayer;

    private List<Vector3> enemyMovePaternPointsPositions = new List<Vector3>();
    
    private EnemyStats baseEnemyStats;

    private int actualPaternPointIndex;
    private int playerChasingWaypointIndex;

    private Vector2 movingDirection;
    private bool isChasingPlayer;
    private bool enemyIsDefeated;
    private Transform playerTransform;

    private Path actualChasingPath;
    private Coroutine chasingCoroutineActive;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        ConfigureCharacter();
    }

    private void FixedUpdate()
    {
        if (!LightManThiefSingleton.GameplayIsRunning || enemyIsDefeated)
        {
            MoveCharacter(Vector3.zero);
            StopPlayerChasing();
            return;
        }

        MoveEnemy(isChasingPlayer);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (chasingCoroutineActive != null)
        {
            return;
        }

        if ((wallLayer.value & 1 << collision.gameObject.layer) == 1 << collision.gameObject.layer)
        {
            StopAllCoroutines();
            StartCoroutine(ReturnToBasePattern());
        }
    }

    #endregion

    #region Base Character inheritance

    protected override void ConfigureCharacter()
    {
        actualPaternPointIndex = -1;
        playerChasingWaypointIndex = -1;
        movingDirection = Vector2.zero;

        baseEnemyStats = characterStatsData as EnemyStats;
        enemyLanternLight.color = baseEnemyStats.LanternColor;
        detectionLinearDistance = enemyLanternLight.pointLightDistance;
        actualCharacterSpeed = baseEnemyStats.CharacterBaseSpeed;
        scanRadius = characterStatsData.CharacterDetectionRadius;

        foreach (Transform point in enemyMovePaternPoints)
            enemyMovePaternPointsPositions.Add(point.position);

        isChasingPlayer = false;
        enemyIsDefeated = false;
    }

    public override void SetCharacterDefeated()
    {
        StopEnemy("EnemyDeath");
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Set enemy actual alert state according if player was found by an enemy
    /// </summary>
    /// <param name="playerAlertNewState">Player set alert new state</param>
    /// <param name="playerTransform">Player transform data (if found by this enemy)</param>
    public void SetPlayerAlertState(bool playerAlertNewState, Transform playerTransform = null)
    {
        actualCharacterSpeed = playerAlertNewState ? baseEnemyStats.CharacterRunningSpeed : baseEnemyStats.CharacterBaseSpeed;
        
        if(playerAlertNewState)
            InGameController.Instance.SetNewLightState(true);

        if (playerTransform == null || chasingCoroutineActive != null)
            return;

        //if (playerTransform != null)
        //{
            this.playerTransform = playerTransform;
            StopPlayerChasing();
            //playerChasingWaypointIndex = -1;
            chasingCoroutineActive = StartCoroutine(SetChaseDirection());
        //}
    }

    /// <summary>
    /// Set enemy bribe state
    /// </summary>
    public void OnEnemyBribe(Transform playerTransform)
    {
        if (isChasingPlayer)
            return;

        MoveCharacter(Vector2.zero);
        StopPlayerChasing();
        int randomIndex = Random.Range(0, possibleBribeDialogs.Count);
        DialogScriptableObject dialogToSet = possibleBribeDialogs[randomIndex];
        dialogToSet.BribeMoney = baseEnemyStats.MoneyToBribe;
        InGameController.Instance.SetBribeDialog(dialogToSet, () => { StopEnemy("EnemyBribed"); }, () => { SetPlayerAlertState(true, playerTransform); });
    }

    /// <summary>
    /// Stop player chasing coroutine
    /// </summary>
    public void StopPlayerChasing(bool resetEnemyPosition = false)
    {
        actualPaternPointIndex = -1;

        if (chasingCoroutineActive != null)
        {
            StopCoroutine(chasingCoroutineActive);
            chasingCoroutineActive = null;
        }

        if(!resetEnemyPosition && LightManThiefSingleton.GameplayIsRunning)
            StartCoroutine(ReturnToBasePattern());
        else if(resetEnemyPosition)
        {
            transform.position = enemyMovePaternPointsPositions[0];
            isChasingPlayer = false;
        }
    }

    public void ActivateCollider(bool activeState) => enemyCollider.enabled = activeState;

    #endregion

    #region Private Methods

    /// <summary>
    /// Set enemy direction movement
    /// </summary>
    /// <param name="isChasingPlayer">Enemy is chasing player?</param>
    private void MoveEnemy(bool isChasingPlayer)
    {
        int pointIndex = isChasingPlayer ? playerChasingWaypointIndex : actualPaternPointIndex;

        List<Vector3> actualListPoint = isChasingPlayer ? actualChasingPath.vectorPath : enemyMovePaternPointsPositions;


        if (movingDirection == Vector2.zero || pointIndex <= -1 || Vector3.Distance(transform.position, actualListPoint[pointIndex]) < 0.25f)
        {
            movingDirection = GetNewMoveDirection(isChasingPlayer, pointIndex, actualListPoint);
        }
        else
        {
            MoveCharacter(movingDirection);
        }

        DetectInteractablesInFront(movingDirection);
    }

    /// <summary>
    /// Returns enemy new move direction according to the next point it must approach
    /// </summary>
    /// <param name="isChasingPlayer">Actually enemy is chasing player</param>
    /// <param name="actualPointIndex">Are normal enemy move pattern position points or enemy chasing player position points</param>
    /// <param name="actualListPoint">List of position points (path points)</param>
    /// <returns>New move direction</returns>
    private Vector2 GetNewMoveDirection(bool isChasingPlayer, int actualPointIndex, List<Vector3> actualListPoint)
    {
        actualPointIndex++;

        if (actualPointIndex >= actualListPoint.Count)
            actualPointIndex = 0;

        if (isChasingPlayer)
            playerChasingWaypointIndex = actualPointIndex;
        else
            actualPaternPointIndex = actualPointIndex;

        return (actualListPoint[actualPointIndex] - transform.position).normalized;
    }

    /// <summary>
    /// Chase player until player get catched or run from enemy's sight
    /// </summary>
    /// <returns></returns>
    private IEnumerator SetChaseDirection()
    {
        if(!enemyIsDefeated)
        {
            while (Vector3.Distance(transform.position, playerTransform.position) <= scanRadius + 5f)
            {
                seeker.StartPath(transform.position, playerTransform.position, OnPathComplete);
                yield return new WaitForSeconds(0.5f);
            }

            StopPlayerChasing();
        }

        InGameController.Instance.SetNewLightState(false);
    }

    private IEnumerator ReturnToBasePattern()
    {
        if(!enemyIsDefeated)
        {
            Vector3 nearestPosition = GetNearestPoint();
            seeker.StartPath(transform.position, nearestPosition, OnPathComplete);
            yield return new WaitUntil(() => Vector3.Distance(transform.position, nearestPosition) < 0.25f);
            if(isChasingPlayer)
            {
                actualChasingPath = null;
                isChasingPlayer = false;
            }
        }
    }

    /// <summary>
    /// Set a new path as the actual path
    /// </summary>
    /// <param name="path">New path</param>
    private void OnPathComplete(Path path)
    {
        if(!path.error)
        {
            actualChasingPath = path;
            playerChasingWaypointIndex = -1;
            isChasingPlayer = true;
        }
    }

    private void StopEnemy(string animationNewState)
    {
        MoveCharacter(Vector2.zero);
        enemyIsDefeated = true;
        
        //StopAllCoroutines();
        if(chasingCoroutineActive != null)
            StopCoroutine(chasingCoroutineActive);
        
        characterAnimator.SetTrigger(animationNewState);
        StartCoroutine(HideEnemyAfterAnimation());
        InGameController.Instance.SetNewLightState(false);
    }

    /// <summary>
    /// Hide object after death animation
    /// </summary>
    /// <returns></returns>
    private IEnumerator HideEnemyAfterAnimation()
    {
        ActivateCollider(false);
        yield return new WaitForSeconds(0.7f);
        enemyIsDefeated = false;
        gameObject.SetActive(false);
    }

    private Vector3 GetNearestPoint()
    {
        Vector3 nearestPosition = enemyMovePaternPointsPositions[0];
        float nearestDistance = int.MaxValue;
        for (int i = 0; i < enemyMovePaternPointsPositions.Count; i++)
        {
            float distance = Vector3.Distance(transform.position, enemyMovePaternPointsPositions[i]);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestPosition = enemyMovePaternPointsPositions[i];
                actualPaternPointIndex = i;
            }
        }

        return nearestPosition;
    }

    #endregion
}
