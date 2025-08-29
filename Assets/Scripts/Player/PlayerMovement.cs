using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerManager : MonoBehaviour
{
    private Animator Animator;

    public float moveSpeed = 4f;

    public float jumpForce = 6f;

    private Rigidbody2D rb;

    private int jumpCount;

    private int maxJumps = 0;

    private float moveInput;

    private bool isGrounded = false;

    private float jumpBufferTime = 0.2f;
    private float jumpBufferCounter;

    bool IsFacingRight = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //GetAxis = permite interpolação
        //GetAxisRaw = NÃO permite
        //Na prática, o GetAxisRaw faz com que o movimento seja mais constante, pois,
        //A interpolação faz uma estimativa de algo que está entre um valor e outro
        //Por exemplo, se temos 1 e 0, ao interpolar, o movimento pode ficar em 0.5
        //podendo ter uma aceleração até chegar no valor 1
        moveInput = Input.GetAxisRaw("Horizontal");

        if(moveInput != 0)
        {
            Animator.SetBool("IsRunning", true);
        }
        else
        {
            Animator.SetBool("IsRunning", false);
        }

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
            Animator.SetBool("IsJumping",true);
            jumpCount++;

        }

    }

    private void FixedUpdate()
    {

        // estrutura de if else ternário
        float targetVelocity = moveInput != 0 ? moveInput * moveSpeed : 0f;


        //estrutura padrão
        //if (moveInput != 0)
        //{
        //    // Se moveInput não for 0, multiplicamos moveInput por moveSpeed e atribuímos a targetVelocityX
        //    targetVelocity = moveInput * moveSpeed;
        //}
        //else
        //{
        //    // Se moveInput for 0, atribuimos 0 a targetVelocityX
        //    targetVelocity = 0;
        //}

        rb.linearVelocity = new Vector2(targetVelocity, rb.linearVelocity.y);

        if (isGrounded)
        {

            jumpCount = 0;
            
        }

        isGrounded = false;
    }

    void OnCollisionStay2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Ground"))
        {
            Animator.SetBool("IsJumping", false);
            isGrounded = true;
        }
    }
    void flipSprite()
    {
        if(IsFacingRight && moveInput <0f || !IsFacingRight && moveInput > 0f)
        {
            IsFacingRight = !IsFacingRight;
            Vector3 ls = transform.localScale;
            ls.x *= -1f;
            transform.localScale = ls;
        }
    }
}

