using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Lives Manager, manages whether the player has learned and makes sure they don't just rush through tasks
/// Made by Marco Espinoza
/// Last Update: 4/16/2026
/// </summary>
/// 

[System.Serializable]
public class IntEvent : UnityEvent<int> { }

public class LivesManager : MonoBehaviour
{
    public int startingLives = 5;
    private int currentLives;

    // Event triggered when all lives are lost
    public UnityEvent OnAllLivesLost;
    public UnityEvent OnLifeLost;
    public IntEvent OnLivesChanged;

    void Start()
    {
        currentLives = startingLives;
        OnLivesChanged?.Invoke(currentLives);

    }

    public void LoseLife()
    {
        currentLives--;
        Debug.Log("Life lost! Lives remaining: " + currentLives);

        OnLivesChanged?.Invoke(currentLives);
        OnLifeLost?.Invoke();

        if (currentLives <= 0)
        {
            Debug.Log("Game Over!");
            OnAllLivesLost?.Invoke();
        }
    }

    // Resets lives to starting value
    public void ResetLives()
    {
        currentLives = startingLives;
        Debug.Log("Lives reset to: " + currentLives);
        OnLivesChanged?.Invoke(currentLives);
    }

    // Optional: expose current lives for other scripts
    public int GetCurrentLives()
    {
        return currentLives;
    }
}
