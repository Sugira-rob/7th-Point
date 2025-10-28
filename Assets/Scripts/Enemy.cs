using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Stats")]
    public float moveSpeed = 2f;
    public float maxHealth = 50f;
    private float currentHealth;

    [Header("Detection")]
    public Transform player;
    public float detectionRange = 5f;
    public float attackRange = 1f;
    public float attackDamage = 15f;
    public float attackCooldown = 1.5f;
    private float nextAttackTime = 0f;

    private Rigidbody2D rb;
    private Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance < detectionRange)
        {
            ChasePlayer();

            if (distance <= attackRange && Time.time >= nextAttackTime)
            {
                Attack();
                nextAttackTime = Time.time + attackCooldown;
            }
        }
    }

    void ChasePlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = new Vector2(direction.x * moveSpeed, rb.linearVelocity.y);

        if (anim != null)
            anim.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x));

        transform.localScale = new Vector3(Mathf.Sign(direction.x), 1, 1);
    }

    void Attack()
    {
        if (anim != null)
            anim.SetTrigger("Attack");

        // Apply damage if player is close
        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= attackRange)
        {
            player.GetComponent<PlayerController>()?.SendMessage("TakeDamage", attackDamage, SendMessageOptions.DontRequireReceiver);
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        if (anim != null)
            anim.SetTrigger("Die");
        Destroy(gameObject, 0.5f);
    }
}
