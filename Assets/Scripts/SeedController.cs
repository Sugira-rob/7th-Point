using UnityEngine;

public class SeedController : MonoBehaviour
{
    [Header("Follow Settings")]
    public Transform player;
    public float followSpeed = 3f;
    public float followDistance = 1.5f;

    [Header("Growth Settings")]
    public int growthStage = 0; // 0 = small, 1 = sapling, 2 = tree
    public Sprite[] growthSprites; // Assign in Inspector
    private SpriteRenderer spriteRenderer;

    [Header("Health Settings")]
    public float maxHealth = 100f;
    private float currentHealth;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
        UpdateGrowthVisual();
    }

    void Update()
    {
        FollowPlayer();
    }

    void FollowPlayer()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);
        if (distance > followDistance)
        {
            Vector2 newPos = Vector2.Lerp(transform.position, player.position, followSpeed * Time.deltaTime);
            transform.position = newPos;
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0) Die();
    }

    public void Heal(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }

    public void Grow()
    {
        if (growthStage < growthSprites.Length - 1)
        {
            growthStage++;
            UpdateGrowthVisual();
        }
    }

    void UpdateGrowthVisual()
    {
        if (growthSprites.Length > 0 && growthStage < growthSprites.Length)
            spriteRenderer.sprite = growthSprites[growthStage];
    }

    void Die()
    {
        Debug.Log("The seed has perished...");
        gameObject.SetActive(false);
    }
}
