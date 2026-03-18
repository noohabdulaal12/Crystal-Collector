using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 5, -10);
    public float smoothSpeed = 0.125f;

    public float sensitivity = 0.2f;
    public float pitch = 0f;
    public float yaw = 0f; // New variable for horizontal rotation

    void LateUpdate()
    {
        if (target == null) return;

        // 1. Handle Mouse Input
        if (Pointer.current != null)
        {
            Vector2 mouseDelta = Pointer.current.delta.ReadValue();

            // Horizontal rotation (Yaw)
            yaw += mouseDelta.x * sensitivity;

            // Vertical rotation (Pitch)
            pitch -= mouseDelta.y * sensitivity;
            pitch = Mathf.Clamp(pitch, -40f, 40f);
        }

        target.rotation = Quaternion.Euler(0, yaw, 0);

        // 2. Calculate the Rotation
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);

        // 3. Position the Camera (Orbiting)
        // This math calculates a point at 'offset' distance away, rotated by our mouse
        Vector3 desiredPosition = target.position + (rotation * offset);
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // 4. Always look at the player
        transform.LookAt(target.position + Vector3.up * 1.5f); // Look slightly above feet
    }
}