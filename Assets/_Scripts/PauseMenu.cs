using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [Header("Assign your pause menu UI panel here")]
    public GameObject pauseMenuPanel;

    private bool isPaused = false;

    void Start()
    {
        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(false); // Start hidden
    }


    // This method should be called by your Active Input event
    public void TogglePauseMenu()
    {
        isPaused = !isPaused;
        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(isPaused);

        Time.timeScale = isPaused ? 0f : 1f;
    }

    // Optional: Call this from a UI button to resume
    public void Resume()
    {
        isPaused = false;
        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(false);
        Time.timeScale = 1f;
    }
}
