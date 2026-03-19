using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 8f;
    public float gravity = -15f;
    public float jumpHeight = 2.5f;

    Vector3 velocity;
    bool isGrounded; // Is the player on the ground? 
    
    public Transform modelAnchor;

    private void Start()
    {
        Application.targetFrameRate = 30; // FPS kept low for performance purposes, later I can make it adjustable in settings
    }

    void Update()
    {
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float moveX = 0;
        float moveZ = 0;

        if (Keyboard.current.wKey.isPressed) moveZ = 1;
        if (Keyboard.current.sKey.isPressed) moveZ = -1;
        if (Keyboard.current.aKey.isPressed) moveX = -1;
        if (Keyboard.current.dKey.isPressed) moveX = 1;

        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 move = (camForward * moveZ) + (camRight * moveX);

        controller.Move(speed * Time.deltaTime * move);

        if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        if (!isGrounded && controller.velocity.y == 0 && velocity.y > 0)
        {
            velocity.y = 0;
        }
    }

    // I plan to add new characters later

    public void ChangeShape(GameObject newShapePrefab)
    {
        foreach (Transform child in modelAnchor)
        {
            Destroy(child.gameObject);
        }

        Instantiate(newShapePrefab, modelAnchor.position, modelAnchor.rotation, modelAnchor);
    }
}