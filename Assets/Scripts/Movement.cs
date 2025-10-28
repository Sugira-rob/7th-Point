using UnityEngine;

public class SideScrollPlayerController : MonoBehaviour
{
    public float moveSpeed = 10.0f;

    public float jumpForce = 500.0f;

    Rigidbody2D rb;

    public bool isGrounded = false;

    public bool shouldJump = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // get horizontal input
        float horizontalInput = Input.GetAxis("Horizontal");

        transform.Translate(new Vector3(horizontalInput, 0, 0) * moveSpeed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            shouldJump = true;
        }
    }

    void FixedUpdate()
    {
        if (shouldJump == true)
        {
            // quickly set back to false so we don't double-jump
            shouldJump = false;

            //push the rigidbody UP
            rb.AddForce(transform.up * jumpForce);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
            isGrounded = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
            isGrounded = false;
    }
}