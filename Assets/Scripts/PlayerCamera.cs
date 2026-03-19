using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    public Transform target; // To rreference player
    public Vector3 offset = new Vector3(0, 5, -10); // Distance between camera and player
    public float smoothSpeed = 1f; // How fast the camera catches up to the player

    public float sensitivity = 0.2f; // Multiplier for mouse movement 
    public float pitch = 0f; // Initial setting for camera for X axis
    public float yaw = 0f; // Initial setting for camera for Y axis
    public float lookHeight = 1.5f; // Additive for camera Y axis

    void LateUpdate()
    {
        if (target == null) return;

        if (Pointer.current != null)
        {
            // Change in mouse
            Vector2 mouseDelta = Pointer.current.delta.ReadValue();

            // Horizontal rotation (Yaw)
            yaw += mouseDelta.x * sensitivity;

            // Vertical rotation (Pitch)
            pitch -= mouseDelta.y * sensitivity;
            pitch = Mathf.Clamp(pitch, -40f, 40f);
        }

        target.rotation = Quaternion.Euler(0, yaw, 0);

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);

        Vector3 desiredPosition = target.position + (rotation * offset);
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        transform.LookAt(target.position + Vector3.up * lookHeight); 
    }
}