using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField] private GameObject slashAnimPrefab;
    [SerializeField] private Transform slashAnimSpawnPoint;
    private PlayerControlls playerControls;
    private Animator myAnimator;
    private PlayerController playerController;

    private GameObject slashAnim;

    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
        playerControls = new PlayerControlls();
        // Try to find a PlayerController on this object or a parent, otherwise fall back to any in the scene.
        playerController = GetComponentInParent<PlayerController>();
        if (playerController == null)
        {
            playerController = FindObjectOfType<PlayerController>();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }
    }

    private void Attack()
    {
        // Trigger animator if present
        if (myAnimator != null)
        {
            myAnimator.SetTrigger("Attack");
        }

        // Spawn slash animation prefab at the spawn point
        if (slashAnimPrefab != null && slashAnimSpawnPoint != null)
        {
            slashAnim = Instantiate(slashAnimPrefab, slashAnimSpawnPoint.position, Quaternion.identity);

            // Apply facing flip if needed
            var sr = slashAnim.GetComponent<SpriteRenderer>();
            if (sr != null && playerController != null && playerController.FacingLeft)
            {
                sr.flipX = true;
            }
        }
    }

    public void SwingUpFLipAnim()
    {
        if (slashAnim == null) return;

        slashAnim.transform.rotation = Quaternion.Euler(-180, 0, 0);

        if (playerController != null && playerController.FacingLeft)
        {
            var sr = slashAnim.GetComponent<SpriteRenderer>();
            if (sr != null) sr.flipX = true;
        }
    }

    public void SwingDownFlipAnim()
    {
        if (slashAnim == null) return;

        slashAnim.transform.rotation = Quaternion.Euler(0, 0, 0);

        if (playerController != null && playerController.FacingLeft)
        {
            var sr = slashAnim.GetComponent<SpriteRenderer>();
            if (sr != null) sr.flipX = true;
        }
    }
}

