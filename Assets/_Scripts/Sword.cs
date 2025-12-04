using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField] private GameObject slashAnimPrefab;
    [SerializeField] private Transform slashAnimSpawnPoint;
    [SerializeField] private Transform weaponCollider;

    private PlayerControlls playerControls;
    private Animator myAnimator;
    private PlayerController playerController;

    private GameObject slashAnim;

    // Store the original local positions so we can mirror them
    private Vector3 weaponColliderDefaultLocalPos;
    private Vector3 slashSpawnDefaultLocalPos;

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

        // Cache default local positions (these should be set up for "facing right" in the editor)
        if (weaponCollider != null)
        {
            weaponColliderDefaultLocalPos = weaponCollider.localPosition;
            weaponCollider.localRotation = Quaternion.Euler(0, 0, 0);
        }

        if (slashAnimSpawnPoint != null)
        {
            slashSpawnDefaultLocalPos = slashAnimSpawnPoint.localPosition;
        }
    }

    private void Update()
    {
        // Make collider + slash spawn move to the correct side of the player
        UpdateSide();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }
    }

    // --------------------------
    // Move collider & spawn left/right
    // --------------------------
    private void UpdateSide()
    {
        if (playerController == null) return;

        bool facingLeft = playerController.FacingLeft;

        // Mirror the weapon collider position on X
        if (weaponCollider != null)
        {
            float x = Mathf.Abs(weaponColliderDefaultLocalPos.x);
            if (facingLeft) x = -x;

            weaponCollider.localPosition = new Vector3(
                x,
                weaponColliderDefaultLocalPos.y,
                weaponColliderDefaultLocalPos.z
            );
        }

        // Mirror the slash spawn position on X
        if (slashAnimSpawnPoint != null)
        {
            float x = Mathf.Abs(slashSpawnDefaultLocalPos.x);
            if (facingLeft) x = -x;

            slashAnimSpawnPoint.localPosition = new Vector3(
                x,
                slashSpawnDefaultLocalPos.y,
                slashSpawnDefaultLocalPos.z
            );
        }
    }

    // --------------------------
    // ATTACK
    // --------------------------
    private void Attack()
    {
        // Trigger animator if present
        if (myAnimator != null)
        {
            myAnimator.SetTrigger("Attack");
            weaponCollider.gameObject.SetActive(true);
        }

        // Spawn slash animation prefab at the (now mirrored) spawn point
        if (slashAnimPrefab != null && slashAnimSpawnPoint != null)
        {
            slashAnim = Instantiate(
                slashAnimPrefab,
                slashAnimSpawnPoint.position,
                Quaternion.identity
            );

            // Flip the sprite if facing left
            var sr = slashAnim.GetComponent<SpriteRenderer>();
            if (sr != null && playerController != null && playerController.FacingLeft)
            {
                sr.flipX = true;
            }
        }
    }

    private void DoneAttackingAnimEvent()
    {
        if (weaponCollider != null)
        {
            weaponCollider.gameObject.SetActive(false);
        }
    }

    // --------------------------
    // SWING UP / DOWN (ANIM EVENTS)
    // --------------------------
    public void SwingUpFLipAnimEvent()
    {
        if (slashAnim == null) return;

        // This just controls arc orientation; side is handled by spawn position
        slashAnim.transform.rotation = Quaternion.Euler(-180, 0, 0);

        if (playerController != null && playerController.FacingLeft)
        {
            var sr = slashAnim.GetComponent<SpriteRenderer>();
            if (sr != null) sr.flipX = true;
        }
    }

    public void SwingDownFlipAnimEvent()
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
