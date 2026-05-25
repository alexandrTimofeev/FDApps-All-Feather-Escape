using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuCanvas;
    public GameObject hideOnPausePanel; // Reference to the HideOnPause panel
    public GameObject planButton;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }

    public void TogglePauseMenu()
    {
        bool isActive = pauseMenuCanvas.activeSelf;
        pauseMenuCanvas.SetActive(!isActive);

        if (isActive)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void ResumeGame()
    {
        pauseMenuCanvas.SetActive(false);
        Time.timeScale = 1f;
        if (hideOnPausePanel != null)
        {
            hideOnPausePanel.SetActive(true); // Re-enable the HideOnPause panel when resuming
            planButton.SetActive(true);
        }
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }

    private void PauseGame()
    {
        Time.timeScale = 0f;
        if (hideOnPausePanel != null)
        {
            hideOnPausePanel.SetActive(false); // Disable the HideOnPause panel when pausing
            planButton.SetActive(false);
        }
    }
}
