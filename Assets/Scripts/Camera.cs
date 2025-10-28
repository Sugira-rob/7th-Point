using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform target; // Player or object to follow
    public Vector3 offset;   // Adjust camera position relative to player

    [Header("Smoothness Settings")]
    public float smoothSpeed = 0.125f; // Smaller = smoother

    void LateUpdate()
    {
        if (target == null) return;

        // Desired position
        Vector3 desiredPosition = target.position + offset;

        // Smoothly interpolate between current and target position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Apply to camera
        transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, transform.position.z);
    }
}
