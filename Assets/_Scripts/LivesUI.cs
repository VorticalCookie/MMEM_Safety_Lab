using UnityEngine;
using TMPro;

public class LivesUI : MonoBehaviour
{
    public LivesManager livesManager;
    public TextMeshProUGUI livesText;

    void Awake()
    {
        // Subscribe to the event
        if (livesManager != null)
            livesManager.OnLivesChanged.AddListener(UpdateLivesText);
    }

    void Start()
    {
        // Initialize UI
        if (livesManager != null)
            UpdateLivesText(livesManager.GetCurrentLives());
    }

    void OnDestroy()
    {
        // Unsubscribe to prevent memory leaks
        if (livesManager != null)
            livesManager.OnLivesChanged.RemoveListener(UpdateLivesText);
    }

    void UpdateLivesText(int lives)
    {
        livesText.text = lives + " / 5 " ;
    }
}
