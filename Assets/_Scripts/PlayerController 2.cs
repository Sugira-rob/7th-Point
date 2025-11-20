using UnityEngine;

public class SideScrollPlayer : MonoBehaviour
{
    public float moveSpeed = 10.0f;
    public float jumpForce = 500.0f;

    private Rigidbody2D rb;
    private bool isGrounded = false;
    private bool shouldJump = false;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        // Flip sprite & animate running
        if (horizontalInput > 0)
        {
            animator.SetBool("isRunning", true);
            spriteRenderer.flipX = false;
        }
        else if (horizontalInput < 0)
        {
            animator.SetBool("isRunning", true);
            spriteRenderer.flipX = true;
        }
        else
        {
            animator.SetBool("isRunning", false);
        }

        // Jump input
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            shouldJump = true;
        }
    }

    void FixedUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        // ✅ Move using Rigidbody2D velocity (physics-friendly)
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);

        // ✅ Jump force
        if (shouldJump)
        {
            shouldJump = false;
            rb.AddForce(Vector2.up * jumpForce);
            animator.SetBool("isJumping", true);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        isGrounded = true;
        animator.SetBool("isJumping", false);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        isGrounded = false;
    }
}
