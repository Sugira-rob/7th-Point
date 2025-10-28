using UnityEngine;

public class WaterSplash : MonoBehaviour
{
    [Header("Splash Settings")]
    public GameObject splashEffect; // Assign a particle prefab here
    public float splashOffsetY = 0.2f; // Slight offset above water surface
    public AudioClip splashSound; // Optional: add splash sound

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Body"))
        {
            CreateSplash(other.transform.position);
        }
    }

    void CreateSplash(Vector3 position)
    {
        // Adjust splash position slightly above water surface
        Vector3 splashPos = new Vector3(position.x, transform.position.y + splashOffsetY, position.z);

        // Create the particle effect
        if (splashEffect != null)
        {
            GameObject splash = Instantiate(splashEffect, splashPos, Quaternion.identity);
            Destroy(splash, 2f); // cleanup after 2 seconds
        }

        // Play splash sound if available
        if (splashSound != null)
        {
            AudioSource.PlayClipAtPoint(splashSound, splashPos);
        }
    }
}
