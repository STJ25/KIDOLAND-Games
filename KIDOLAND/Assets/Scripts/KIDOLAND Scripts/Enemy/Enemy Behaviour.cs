using UnityEngine;
using System.Collections.Generic;

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

    [Header("Data")]
    [SerializeField] private EnemyData data;

    [Header("Patrol")]
    [SerializeField] private List<Transform> patrolPoints;

    private Rigidbody2D rb;
    private Transform player;

    private EnemyState currentState;

    private int currentPatrolIndex;
    private Transform currentTargetPoint;


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
                HandlePatrol();
                break;

            case EnemyState.Chase:
                HandleChase();
                break;

            case EnemyState.ReturnToPatrol:
                HandleReturn();
                break;
        }
    }

    private void HandlePatrol()
    {
        if (currentTargetPoint == null) return;

        MoveTowards(currentTargetPoint.position, data.moveSpeed);

        if (Vector2.Distance(transform.position, currentTargetPoint.position) < 0.1f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count;
            currentTargetPoint = patrolPoints[currentPatrolIndex];
        }
    }

    private void HandleChase()
    {
        if (player == null) return;

        float speed = data.moveSpeed * data.chaseMultiplier;
        MoveTowards(player.position, speed);
    }

    private void HandleReturn()
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

    // ===== EVENT HOOKS =====

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

    // ===== COLLISION =====

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
}