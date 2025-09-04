using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerManager : MonoBehaviour
{
    // === Referências ===
    private Animator animator;
    private Rigidbody2D rb;

    [Header("Movimento")]
    public float walkSpeed = 4f;       // Velocidade base andando
    public float runSpeed = 8f;        // Velocidade máxima correndo
    public float accelerationTime = 2f; // Tempo segurando direção até atingir velocidade máxima
    private float holdTime = 0f;       // Tempo segurando direção
    private float currentSpeed;        // Velocidade atual
    private float moveInput;           // Entrada horizontal
    private bool isFacingRight = true; // Direção do sprite

    [Header("Pulo")]
    public float jumpForce = 6f;
    private int jumpCount;
    private int maxJumps = 0;

    // Jump Buffer
    private float jumpBufferTime = 0.2f;
    private float jumpBufferCounter;


    // Controle de estados
    private bool wasRunning = false;
    private int lastDirection = 0; // -1 esquerda, 1 direita

    [Header("Ground Check (2 Raycasts)")]
    public Transform leftCheck;         // Pé esquerdo
    public Transform rightCheck;        // Pé direito
    public float groundCheckDistance = 0.2f;
    public LayerMask groundLayer;
    public bool isGrounded;

    [Header("Wall Check")]
    public LayerMask WallLayer;
    public Transform WallCheckUp;
    public Transform WallCheckDown;
    public float WallCheckDistance = 0.2f;
    public bool IsWallRunning = false;
    public float WallRunSpeed;
    // ==================== UNITY ====================

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentSpeed = walkSpeed;
    }

    void Update()
    {
        HandleInput();
        HandleAnimations();
        HandleJumpBuffer();
        FlipSprite();
    }

    private void FixedUpdate()
    {
        ApplyMovement();
        CheckGrounded();
        CheckWallRunning();
        CheckFalling();
    }

    private void OnDrawGizmosSelected()
    {
        if (leftCheck != null)
            Gizmos.DrawLine(leftCheck.position, leftCheck.position + Vector3.down * groundCheckDistance);

        if (rightCheck != null)
            Gizmos.DrawLine(rightCheck.position, rightCheck.position + Vector3.down * groundCheckDistance);
    }

    // ==================== MOVIMENTO ====================

    void HandleInput()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
    }

    void ApplyMovement()
    {
        float targetVelocity = moveInput * currentSpeed;
        rb.linearVelocity = new Vector2(targetVelocity, rb.linearVelocity.y);

        if (isGrounded)
            jumpCount = 0;
    }

    // ==================== ANIMAÇÕES ====================

    void HandleAnimations()
    {
        if (moveInput != 0)
        {
            holdTime += Time.deltaTime;

            float t = Mathf.Clamp01(holdTime / accelerationTime);
            currentSpeed = Mathf.Lerp(walkSpeed, runSpeed, t);

            bool isRunning = currentSpeed >= (runSpeed - 0.1f);
            animator.SetBool("IsWalking", !isRunning);
            animator.SetBool("IsRunning", isRunning);

            // Multiplicador de velocidade da animação
            float animSpeedMultiplier = Mathf.Lerp(1f, 1.5f, t);
            animator.SetFloat("SpeedMultiplier", animSpeedMultiplier);
        }
        else
        {
            holdTime = 0f;
            currentSpeed = walkSpeed;

            animator.SetBool("IsWalking", false);
            animator.SetBool("IsRunning", false);
            animator.SetFloat("SpeedMultiplier", 1f);
        }
    }

    // ==================== PULO ====================

    void HandleJumpBuffer()
    {
        if (Input.GetButtonDown("Jump"))
            jumpBufferCounter = jumpBufferTime;
        else
            jumpBufferCounter -= Time.deltaTime;

        if (jumpBufferCounter > 0 && isGrounded)
        {
            jumpBufferCounter = 0;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

            animator.SetTrigger("IsJumping");
            jumpCount++;
        }
    }
    void CheckFalling()
    {
       if (rb.linearVelocityY < 0.1f)
        {
            animator.SetBool("IsFalling", true);
        }
        else
        {
            animator.SetBool("IsFalling", false);
        }
    }

    void CheckGrounded()
    {
        bool leftHit = Physics2D.Raycast(leftCheck.position, Vector2.down, groundCheckDistance, groundLayer);
        bool rightHit = Physics2D.Raycast(rightCheck.position, Vector2.down, groundCheckDistance, groundLayer);

        isGrounded = leftHit || rightHit;

        if ( isGrounded == true)
        {
            animator.SetBool("IsGrounded", true);
        }
        else
        {
            animator.SetBool("IsGrounded", false);
        }

    }
    void CheckWallRunning()
    {
        Vector2 direction;

        if (isFacingRight == true)
            direction = Vector2.right;
        else
            direction = Vector2.left;


        bool WallHitUP = Physics2D.Raycast(WallCheckUp.position, direction, WallCheckDistance, WallLayer);
        bool WallHitDown = Physics2D.Raycast(WallCheckDown.position, direction, WallCheckDistance, WallLayer);

        if (currentSpeed == runSpeed && WallHitDown == true || WallHitUP == true )
        {
            IsWallRunning = true;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 10f);
        }
        else
        {
            IsWallRunning = false;
        }

    }

    // ==================== SPRITE ====================

    void FlipSprite()
    {
        if ((isFacingRight && moveInput < 0f) || (!isFacingRight && moveInput > 0f))
        {
            isFacingRight = !isFacingRight;
            Vector3 ls = transform.localScale;
            ls.x *= -1f;
            transform.localScale = ls;
        }
    }
}