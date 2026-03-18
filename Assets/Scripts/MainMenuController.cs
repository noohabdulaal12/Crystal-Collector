using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public GameObject player;
    private void Start()
    {

        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            // 1. Teleport to where we left off
            if (player != null && PlayerPrefs.HasKey("SavedX"))
            {
                LoadPlayerPosition();
            }

            // 2. Lock the mouse for the 360 camera
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            // 3. Ensure time is running
            Time.timeScale = 1f;
        }
    }

    void Update()
    {
        // 4. Treat Escape as the "Pause & Return" trigger
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            // Only trigger if we are currently in the Game (Scene 1)
            if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                PauseAndReturnToMenu();
            }
        }
    }
    public void StartGame()
    {
        // This is called by your "Play" button in Scene 0
        SceneManager.LoadScene(1);
    }

    public void PauseAndReturnToMenu()
    {
        // 5. Save the spot on the 1000x1000 grid
        if (player != null)
        {
            PlayerPrefs.SetFloat("SavedX", player.transform.position.x);
            PlayerPrefs.SetFloat("SavedY", player.transform.position.y);
            PlayerPrefs.SetFloat("SavedZ", player.transform.position.z);

            PlayerCamera camScript = Camera.main.GetComponent<PlayerCamera>();
            if (camScript != null)
            {
                PlayerPrefs.SetFloat("SavedPitch", camScript.pitch);
                PlayerPrefs.SetFloat("SavedYaw", camScript.yaw);
            }

            PlayerPrefs.Save();
        }

        // 6. Unlock mouse before leaving
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // 7. Go to Menu
        SceneManager.LoadScene(0);
    }
    private void LoadPlayerPosition()
    {
        PlayerCamera camScript = Camera.main.GetComponent<PlayerCamera>();

        // 1. Teleport Player
        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        player.transform.position = new Vector3(
            PlayerPrefs.GetFloat("SavedX"),
            PlayerPrefs.GetFloat("SavedY"),
            PlayerPrefs.GetFloat("SavedZ")
        );

        if (cc != null) cc.enabled = true;

        // 2. Sync the Camera Script
        if (camScript != null)
        {
            camScript.pitch = PlayerPrefs.GetFloat("SavedPitch");
            camScript.yaw = PlayerPrefs.GetFloat("SavedYaw");

            // Force the camera to jump to the new spot immediately 
            // so it doesn't "slide" across the 1000x1000 map
            Quaternion rotation = Quaternion.Euler(camScript.pitch, camScript.yaw, 0);
            transform.position = player.transform.position + (rotation * camScript.offset);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}