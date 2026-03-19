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
            if (player != null && PlayerPrefs.HasKey("SavedX"))
            {
                LoadPlayerPosition();
            }

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            Time.timeScale = 1f;
        }
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                PauseAndReturnToMenu();
            }
        }
    }
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void PauseAndReturnToMenu()
    {
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

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SceneManager.LoadScene(0);
    }
    private void LoadPlayerPosition()
    {
        PlayerCamera playerCamera = Camera.main.GetComponent<PlayerCamera>();

        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        player.transform.position = new Vector3(
            PlayerPrefs.GetFloat("SavedX"),
            PlayerPrefs.GetFloat("SavedY"),
            PlayerPrefs.GetFloat("SavedZ")
        );

        if (cc != null) cc.enabled = true;

        if (playerCamera != null)
        {
            playerCamera.pitch = PlayerPrefs.GetFloat("SavedPitch");
            playerCamera.yaw = PlayerPrefs.GetFloat("SavedYaw");

            Quaternion rotation = Quaternion.Euler(playerCamera.pitch, playerCamera.yaw, 0);
            transform.position = player.transform.position + (rotation * playerCamera.offset);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}