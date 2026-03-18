using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 8f;
    public float gravity = -15f;
    public float jumpHeight = 2.5f;

    Vector3 velocity;
    bool isGrounded;

    // This is the slot where your current shape (Cube/Sphere) lives
    public Transform modelAnchor;

    private void Start()
    {
        Application.targetFrameRate = 30;
    }

    void Update()
    {
        // 1. Ground Check (The CharacterController's own detection)
        isGrounded = controller.isGrounded;

        // Reset downward velocity when we are on the floor
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // 2. Get Input for Walking
        float moveX = 0;
        float moveZ = 0;

        if (Keyboard.current.wKey.isPressed) moveZ = 1;
        if (Keyboard.current.sKey.isPressed) moveZ = -1;
        if (Keyboard.current.aKey.isPressed) moveX = -1;
        if (Keyboard.current.dKey.isPressed) moveX = 1;

        // Calculate direction relative to Camera
        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 move = (camForward * moveZ) + (camRight * moveX);

        // MOVE #1: Handle Walking (Horizontal)
        controller.Move(move * speed * Time.deltaTime);

        // 3. Jumping Logic
        if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // 4. Apply Gravity (Always pulling down)
        velocity.y += gravity * Time.deltaTime;

        // MOVE #2: Handle Gravity/Jumping (Vertical)
        // This second Move call ensures gravity never "freezes"
        controller.Move(velocity * Time.deltaTime);

        // 5. THE "UNSTUCK" FIX
        // If we aren't grounded, aren't moving up, but we're in the air... force a fall!
        if (!isGrounded && controller.velocity.y == 0 && velocity.y > 0)
        {
            velocity.y = 0;
        }
    }

    // This function will be called by your Shop later!
    public void ChangeShape(GameObject newShapePrefab)
    {
        // Delete the current shape
        foreach (Transform child in modelAnchor)
        {
            Destroy(child.gameObject);
        }

        // Spawn the new one
        Instantiate(newShapePrefab, modelAnchor.position, modelAnchor.rotation, modelAnchor);
    }
}