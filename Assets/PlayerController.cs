using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 8f;
    private float horizontalInput;

    [Header("Jumping")]
    public float jumpForce = 12f;
    public Transform groundCheck;
    public LayerMask groundLayer;
    private bool isGrounded;

    [Header("Combat")]
    public GameObject bulletPrefab; // 위에서 만든 총알 프리팹 연결
    public Transform firePoint;     // 총알이 발사될 위치 (플레이어 앞)
    private int facingDirection = 1; // 1: 우측, -1: 좌측


    [Header("Advanced Jump Feel")]
    [Tooltip("발판에서 떨어진 후 점프가 허용되는 시간")]
    public float coyoteTime = 0.15f;
    private float coyoteTimeCounter;

    [Tooltip("땅에 닿기 직전 미리 점프를 눌렀을 때 발동되는 시간")]
    public float jumpBufferTime = 0.15f;
    private float jumpBufferCounter;

    // 메트로베니아 능력 해금
    [HideInInspector] public bool hasDoubleJump = false;
    private bool canDoubleJump;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");

        // 1. 바닥 체크
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
            canDoubleJump = true; // 땅에 닿으면 2단 점프 충전
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        // 2. 점프 버퍼링 타이머
        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        // 3. 실제 점프 로직 (코요테 타임 + 버퍼링 적용)
        if (jumpBufferCounter > 0f)
        {
            // 일반 점프 (지상 또는 코요테 타임 내)
            if (coyoteTimeCounter > 0f)
            {
                ExecuteJump();
                jumpBufferCounter = 0f;
            }
            // 2단 점프 (공중에서)
            else if (hasDoubleJump && canDoubleJump)
            {
                ExecuteJump();
                canDoubleJump = false; // 2단 점프 소모
                jumpBufferCounter = 0f;
            }
        }

        // 점프 키를 떼었을 때 상승력을 줄여서 숏점프 구현
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        if (Input.GetKeyDown(KeyCode.J) || Input.GetMouseButtonDown(0))
        {
            Shoot();
        }

        // 좌우 이동할 때 바라보는 방향(facingDirection) 업데이트
        if (horizontalInput > 0) facingDirection = 1;
        else if (horizontalInput < 0) facingDirection = -1;

        // Shoot 함수 추가
        void Shoot()
        {
            if (bulletPrefab != null && firePoint != null)
            {
                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
                Bullet bulletScript = bullet.GetComponent<Bullet>();
                if (bulletScript != null)
                {
                    bulletScript.direction = facingDirection; // 플레이어가 보는 방향으로 설정
                }
            }
        }
    }

    void ExecuteJump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        coyoteTimeCounter = 0f;
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
    }





}