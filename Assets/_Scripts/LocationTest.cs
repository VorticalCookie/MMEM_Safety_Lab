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
    public int targetTriggers = 3; //Change to the number of objects needed for the task

    private int currentTriggers = 0;
    private bool taskCompleted = false;
    private bool timerRunning = false;

    [Header("Timer Settings")]
    public float timeLimit = 120f; // 2 minutes
    public UnityEvent OnTimerStart;
    public UnityEvent OnTimerFail;

    private float timer;

    /// <summary>
    /// Starts the timed event for the location test. 
    /// </summary>
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
        if (!timerRunning || taskCompleted) return;

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            timerRunning = false;
            OnTimerFail?.Invoke();
            Debug.Log("Task failed. Time ran out.");
        }
    }

    /// Increments the trigger count and checks for task completion.
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

    // Decrements the trigger count, ensuring it doesn't go below zero.
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

        Debug.Log("Socket task completed!");
    }
}