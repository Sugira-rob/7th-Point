using UnityEngine;

public class DamageSource : MonoBehaviour
{
    // This class can be expanded with damage properties later
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<EnemyAI>() != null)
        {
            // Example: Log when something is hit
            Debug.Log($"{gameObject.name} hit {other.gameObject.name}");
        }
    }
        
}
