using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerManager : MonoBehaviour
{
    private Animator Animator;
    private Rigidbody2D rb;

    public float walkSpeed = 4f;      // velocidade base andando
    public float runSpeed = 8f;       // velocidade máxima correndo
    public float accelerationTime = 2f; // tempo segurando para atingir velocidade máxima

    public float jumpForce = 6f;

    private float moveInput;
    public bool isGrounded;

    private float jumpBufferTime = 0.2f;
    private float jumpBufferCounter;

    private int jumpCount;
    private int maxJumps = 0;

    private float holdTime = 0f; // tempo segurando direção
    private float currentSpeed;  // velocidade atual

    bool IsFacingRight = true;

    public float slideDuration = 0.3f;   // tempo que o player desliza ao parar
    public float slideFriction = 0.95f;  // desaceleração durante deslize

    private bool isSliding = false;
    private float slideTimer = 0f;

    // Controle de estados
    private bool wasRunning = false;
    private int lastDirection = 0; // -1 esquerda, 1 direita

    [Header("Ground Check (2 Raycasts)")]
    public Transform leftCheck;       // pé esquerdo
    public Transform rightCheck;      // pé direito
    public float groundCheckDistance = 0.2f;
    public LayerMask groundLayer;     // escolha quais layers contam como chão

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        currentSpeed = walkSpeed;
    }

    void Update()
    {
        flipSprite();
        moveInput = Input.GetAxisRaw("Horizontal");

        // Detecta se está andando/correndo
        if (moveInput != 0)
        {
            holdTime += Time.deltaTime;

            float t = Mathf.Clamp01(holdTime / accelerationTime);
            currentSpeed = Mathf.Lerp(walkSpeed, runSpeed, t);

            bool isRunning = currentSpeed >= (runSpeed - 0.1f);
            Animator.SetBool("IsWalking", !isRunning);
            Animator.SetBool("IsRunning", isRunning);

            float animSpeedMultiplier = Mathf.Lerp(1f, 1.5f, t);
            Animator.SetFloat("SpeedMultiplier", animSpeedMultiplier);
        }
        else
        {
            holdTime = 0f;
            currentSpeed = walkSpeed;
            Animator.SetBool("IsWalking", false);
            Animator.SetBool("IsRunning", false);
            Animator.SetFloat("SpeedMultiplier", 1f);
        }

        // Jump buffer
        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if (jumpBufferCounter > 0 && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            Animator.SetBool("IsJumping", true);
            jumpCount++;
        }
    }

    private void FixedUpdate()
    {
        float targetVelocity = moveInput * currentSpeed;
        rb.linearVelocity = new Vector2(targetVelocity, rb.linearVelocity.y);

        if (isGrounded)
        {
            jumpCount = 0;
        }

        CheckGrounded();
    }

    void CheckGrounded()
    {
        bool leftHit = Physics2D.Raycast(leftCheck.position, Vector2.down, groundCheckDistance, groundLayer);
        bool rightHit = Physics2D.Raycast(rightCheck.position, Vector2.down, groundCheckDistance, groundLayer);

        isGrounded = leftHit || rightHit;

        if (isGrounded)
        {
            Animator.SetBool("IsJumping", false);
        }
    }

    void flipSprite()
    {
        if (IsFacingRight && moveInput < 0f || !IsFacingRight && moveInput > 0f)
        {
            IsFacingRight = !IsFacingRight;
            Vector3 ls = transform.localScale;
            ls.x *= -1f;
            transform.localScale = ls;
        }
    }

    // Debug no editor (desenha os raycasts)
    private void OnDrawGizmosSelected()
    {
        if (leftCheck != null)
            Gizmos.DrawLine(leftCheck.position, leftCheck.position + Vector3.down * groundCheckDistance);

        if (rightCheck != null)
            Gizmos.DrawLine(rightCheck.position, rightCheck.position + Vector3.down * groundCheckDistance);
    }
}