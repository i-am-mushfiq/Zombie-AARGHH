using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenu;
    private bool isGamePaused = false;
    private PlayerInputActions playerInputActions;
    public static PauseManager Instance; 

    public delegate void PauseStateChanged(bool isPaused);
    public event PauseStateChanged onPauseStateChanged;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        PlayerController playerController = FindObjectOfType<PlayerController>();
        if (playerController != null)
        {
            playerInputActions = playerController.playerInputActions;
            playerInputActions.Player.Pause.started += ctx => TogglePause();
        }
        else
        {
            Debug.LogError("PlayerController not found! Pause functionality will not work.");
        }
    }

    void OnEnable()
    {
        if (playerInputActions != null)
            playerInputActions.Enable();
    }

    void OnDisable()
    {
        if (playerInputActions != null)
            playerInputActions.Disable();
    }

    void TogglePause()
    {
        if (isGamePaused)
            ContinueGame();
        else
            PauseGame();
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
        isGamePaused = true;

        if (onPauseStateChanged != null)
            onPauseStateChanged(true);

        PlayerController playerController = FindObjectOfType<PlayerController>();
        if (playerController != null)
        {
            playerController.DisableControls();
        }
    }

    public void ContinueGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        isGamePaused = false;

        if (onPauseStateChanged != null)
            onPauseStateChanged(false);

        PlayerController playerController = FindObjectOfType<PlayerController>();
        if (playerController != null)
        {
            playerController.EnableControls();
        }
    }
}
