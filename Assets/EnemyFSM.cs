using UnityEngine;

public class EnemyFSM : MonoBehaviour
{
    public enum EnemyState
    {
        Patrol, // 순찰 상태
        Chase   // 추적 상태
    }

    public EnemyState currentState;

    [Header("Movement Settings")]
    public float moveSpeed = 3f;
    public float patrolDistance = 3f; // 제자리 기준 좌우로 움직일 거리
    private Vector2 startPos;         // 처음 시작한 위치
    private int patrolDirection = 1;  // 1: 우측, -1: 좌측

    [Header("Detection Settings")]
    public Transform player;          // 플레이어 Transform
    public float detectionRange = 5f; // 플레이어를 발견하는 거리

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        startPos = transform.position; // 시작 위치 저장
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentState = EnemyState.Patrol;
    }

    void Update()
    {
        CheckStateTransitions();

        switch (currentState)
        {
            case EnemyState.Patrol:
                PatrolBehavior();
                break;
            case EnemyState.Chase:
                ChaseBehavior();
                break;
        }
    }

    void CheckStateTransitions()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // 플레이어가 범위 안에 들어오면 추적(Chase)으로 전환
        if (distanceToPlayer <= detectionRange)
        {
            if (currentState != EnemyState.Chase)
            {
                currentState = EnemyState.Chase;
                Debug.Log("적 상태 전환: Patrol -> Chase");
            }
        }
        // 플레이어가 멀어지면 다시 순찰(Patrol)로 복귀
        else if (distanceToPlayer > detectionRange * 1.5f)
        {
            if (currentState != EnemyState.Patrol)
            {
                currentState = EnemyState.Patrol;
                Debug.Log("적 상태 전환: Chase -> Patrol");
            }
        }
    }

    void PatrolBehavior()
    {
        // 좌우로 왕복 이동
        transform.Translate(Vector2.right * patrolDirection * moveSpeed * Time.deltaTime);

        // 시작 위치 기준에서 설정한 거리(patrolDistance) 이상 벗어나면 방향 전환
        if (transform.position.x > startPos.x + patrolDistance)
        {
            patrolDirection = -1; // 왼쪽으로
            spriteRenderer.flipX = true;
        }
        else if (transform.position.x < startPos.x - patrolDistance)
        {
            patrolDirection = 1;  // 오른쪽으로
            spriteRenderer.flipX = false;
        }
    }

    void ChaseBehavior()
    {
        if (player == null) return;

        // 플레이어 방향으로 이동
        if (player.position.x > transform.position.x)
        {
            transform.Translate(Vector2.right * moveSpeed * 1.5f * Time.deltaTime);
            spriteRenderer.flipX = false;
        }
        else
        {
            transform.Translate(Vector2.left * moveSpeed * 1.5f * Time.deltaTime);
            spriteRenderer.flipX = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}