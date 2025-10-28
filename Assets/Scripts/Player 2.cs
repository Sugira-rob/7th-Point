using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    private Rigidbody2D rb;
    private bool isGrounded;

    [Header("Combat Settings")]
    public float attackRange = 1f;
    public float attackDamage = 25f;
    public Transform attackPoint;
    public LayerMask enemyLayers;

    private Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        Move();
        if (Input.GetButtonDown("Jump") && isGrounded)
            Jump();

        if (Input.GetKeyDown(KeyCode.F))
            Attack();
    }

    void Move()
    {
        float moveX = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveX * moveSpeed, rb.velocity.y);

        if (moveX != 0)
            transform.localScale = new Vector3(Mathf.Sign(moveX), 1, 1); // Flip sprite

        if (anim != null)
            anim.SetFloat("Speed", Mathf.Abs(moveX));
    }

    void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        isGrounded = false;
        if (anim != null)
            anim.SetTrigger("Jump");
    }

    void Attack()
    {
        if (anim != null)
            anim.SetTrigger("Attack");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyController>()?.TakeDamage(attackDamage);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts[0].normal.y > 0.5f)
            isGrounded = true;
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
