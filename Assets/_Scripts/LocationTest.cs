using TMPro;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Location Test  to check if the player can complete a task by triggering a certain number of times within a time limit.
/// Curent uses are: Fire Extinguisher, Medkits, Eye Wash stations and watherver other location-based task we want to add in the future.
/// Made by Marco Espinoza
/// Last Update: 3/22/2026
/// </summary>
public class LocationTest : MonoBehaviour
{
    [Header("Task Settings")]
    public string taskID;

    [Header("Completion Settings")]
    public int targetTriggers = 3;

    private int currentTriggers = 0;
    private bool taskCompleted = false;
    private bool timerRunning = false;

    [Header("Timer Settings")]
    public float timeLimit = 120f;

    [Header("Events")]
    public UnityEvent OnTimerStart;
    public UnityEvent OnTimerFail;
    public UnityEvent OnTaskComplete;

    [Header("UI")]
    public TMP_Text timerText; 

    private float timer;

    public void StartTimedEvent()
    {
        if (timerRunning || taskCompleted) return;

        timer = timeLimit;
        timerRunning = true;

        OnTimerStart?.Invoke();
        Debug.Log("Timer started.");
    }

    void Update()
    {
        if (!timerRunning || taskCompleted)
        {
            UpdateTimerText();
            return;
        }

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            timerRunning = false;
            OnTimerFail?.Invoke();
            Debug.Log("Task failed. Time ran out.");
        }

        UpdateTimerText();
    }

    public void IncrementTrigger()
    {
        if (!timerRunning || taskCompleted) return;

        currentTriggers++;

        Debug.Log($"Triggers: {currentTriggers}/{targetTriggers}");

        if (currentTriggers >= targetTriggers)
        {
            CompleteTask();
        }
    }

    public void DecrementTrigger()
    {
        if (taskCompleted) return;

        currentTriggers = Mathf.Max(0, currentTriggers - 1);
    }

    private void CompleteTask()
    {
        taskCompleted = true;
        timerRunning = false;

        if (TaskManager.Instance != null)
            TaskManager.Instance.CompleteTask(taskID);

        OnTaskComplete?.Invoke();

        Debug.Log("Socket task completed!");
    }

    private void UpdateTimerText()
    {
        if (timerText != null)
        {
            // Show timer only if running and not completed
            bool show = timerRunning && !taskCompleted;
            if (timerText.gameObject.activeSelf != show)
                timerText.gameObject.SetActive(show);

            if (show)
            {
                int minutes = Mathf.FloorToInt(Mathf.Max(timer, 0) / 60f);
                int seconds = Mathf.FloorToInt(Mathf.Max(timer, 0) % 60f);
                timerText.text = $"{minutes}:{seconds:00} Sec.";
            }
        }
    }
}
