using UnityEngine;

public class EnemyPathfinding : MonoBehaviour
{
    [SerializeField] private float movespeed = 2f;

    private Rigidbody2D rb;
    private Vector2 moveDir;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveDir * (movespeed * Time.fixedDeltaTime));
    }

    public void MoveTo(Vector2 targetPosition)
    {
        moveDir = (targetPosition - rb.position).normalized;
    }
}
