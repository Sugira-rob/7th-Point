using UnityEngine;

public class TopDownPlayer : MonoBehaviour
{
    public bool FacingLeft { get {return FacingLeft;} set {FacingLeft = value;} }
    public float moveSpeed = 5.0f;

    private Rigidbody2D rb;
    private Vector2 movement;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private bool facingLeft = false;
    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // ✅ Get both horizontal and vertical input
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        // Store in a Vector2 for convenience
        movement = new Vector2(horizontalInput, verticalInput).normalized;

        // Optional: animate
        bool isMoving = movement.magnitude > 0;
        animator.SetBool("isRunning", isMoving);

        // Flip sprite if moving left/right
        if (horizontalInput > 0)
            spriteRenderer.flipX = false;
        else if (horizontalInput < 0)
            spriteRenderer.flipX = true;
    }

    void FixedUpdate()
    {
        // ✅ Move in both directions (Cartesian plane)
        rb.linearVelocity = movement * moveSpeed;
    }
}
