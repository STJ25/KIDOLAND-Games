using UnityEngine;
using System.Collections.Generic;
/// <summary>
/// Base Enemy behaviour script. Handles Enemy Behaviour based on the logic from the Enemy data Scriptable object
/// Subscribes to Trigger Game events for player detection in teh trigger area script.
/// Has a Switch case logic for Enemy state along with an enum class for state definitions. Shares similar animations with the PLayer 
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class EnemyBehaviour : MonoBehaviour
{
    public enum EnemyState
    {
        Patrol,
        Chase,
        ReturnToPatrol,
        GameOver
    }
#region Inspector
    [Header("Data")]
    [SerializeField] private EnemyData data;

    [Header("Patrol")]
    [SerializeField] private List<Transform> patrolPoints;

    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private Rigidbody2D rb;
    private Transform player;

    private EnemyState currentState;

    private int currentPatrolIndex;
    private Transform currentTargetPoint;
    private Vector2 lastDirection = Vector2.down; // default Idle direction
#endregion

#region Unity Cycle / Event Subscription
    private void OnEnable()
    {
        EnemyTriggerArea.OnPlayerEnter += OnPlayerEntered;
        EnemyTriggerArea.OnPlayerExit += OnPlayerExited;
    }

    private void OnDisable()
    {
        EnemyTriggerArea.OnPlayerEnter -= OnPlayerEntered;
        EnemyTriggerArea.OnPlayerExit -= OnPlayerExited;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        currentState = EnemyState.Patrol;
        currentPatrolIndex = 0;

        if (patrolPoints.Count > 0)
            currentTargetPoint = patrolPoints[currentPatrolIndex];
    }

    private void FixedUpdate()
    {
        if (currentState == EnemyState.GameOver)
        return;

        switch (currentState)
        {
            case EnemyState.Patrol:
                Patrol();
                break;

            case EnemyState.Chase:
                Chase();
                break;

            case EnemyState.ReturnToPatrol:
                Return();
                break;
        }

        UpdateAnimation();
    }
#endregion

#region Enemy Functionalities and Events 
    private void Patrol()
    {
        if (currentTargetPoint == null) return;

        MoveTowards(currentTargetPoint.position, data.moveSpeed);

        if (Vector2.Distance(transform.position, currentTargetPoint.position) < 0.1f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count;
            currentTargetPoint = patrolPoints[currentPatrolIndex];
        }
    }

    private void Chase()
    {
        if (player == null) return;

        float speed = data.moveSpeed * data.chaseMultiplier;
        MoveTowards(player.position, speed);
    }

    private void Return()
    {
        if (currentTargetPoint == null) return;

        MoveTowards(currentTargetPoint.position, data.moveSpeed);

        if (Vector2.Distance(transform.position, currentTargetPoint.position) < 0.1f)
        {
            currentState = EnemyState.Patrol;
        }
    }

    private void MoveTowards(Vector2 target, float speed)
    {
        Vector2 direction = (target - (Vector2)transform.position).normalized;
        rb.linearVelocity = direction * speed;
    }

    private void SetNearestPatrolPoint()
    {
        float minDist = float.MaxValue;
        int nearestIndex = 0;

        for (int i = 0; i < patrolPoints.Count; i++)
        {
            float dist = Vector2.Distance(transform.position, patrolPoints[i].position);
            if (dist < minDist)
            {
                minDist = dist;
                nearestIndex = i;
            }
        }

        currentPatrolIndex = nearestIndex;
        currentTargetPoint = patrolPoints[currentPatrolIndex];
    }
#endregion

#region Player detection events
    public void OnPlayerEntered(Transform playerTransform)
    {
        player = playerTransform;
        currentState = EnemyState.Chase;
    }

    public void OnPlayerExited()
    {
        player = null;

        SetNearestPatrolPoint();
        currentState = EnemyState.ReturnToPatrol;
    }

#endregion

#region Collision Game over Logic
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Game Over");
            currentState = EnemyState.GameOver;
            rb.linearVelocity = Vector2.zero;

            GameEvents.TriggerGameOver();
        }
    }
#endregion

#region Animation
    private Vector2 GetSnappedDirection(Vector2 dir) //Direct copy from the Player
    {
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        if (angle < 0) angle += 360f;

        if (angle >= 337.5f || angle < 22.5f) return Vector2.right;
        if (angle < 67.5f) return new Vector2(1, 1).normalized;
        if (angle < 112.5f) return Vector2.up;
        if (angle < 157.5f) return new Vector2(-1, 1).normalized;
        if (angle < 202.5f) return Vector2.left;
        if (angle < 247.5f) return new Vector2(-1, -1).normalized;
        if (angle < 292.5f) return Vector2.down;
        return new Vector2(1, -1).normalized;
    }

    private void UpdateAnimation() // same as Player
    {
        Vector2 velocity = rb.linearVelocity;

        bool isMoving = velocity.sqrMagnitude > 0.0025f; // ~0.05 threshold

        if (isMoving)
        {
            Vector2 dir = velocity.normalized;
            lastDirection = GetSnappedDirection(dir);
        }

        animator.SetFloat("moveX", lastDirection.x);
        animator.SetFloat("moveY", lastDirection.y);
        animator.SetBool("isMoving", isMoving);

        if (lastDirection.x != 0)
            spriteRenderer.flipX = lastDirection.x > 0;
    }
#endregion
}