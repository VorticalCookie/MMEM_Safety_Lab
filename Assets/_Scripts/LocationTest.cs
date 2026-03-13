using UnityEngine;
using UnityEngine.Events;

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
    public float timeLimit = 120f; // 2 minutes
    public UnityEvent OnTimerStart;
    public UnityEvent OnTimerFail;

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
        if (!timerRunning || taskCompleted) return;

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            timerRunning = false;
            OnTimerFail?.Invoke();
            Debug.Log("Task failed. Time ran out.");
        }
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

        Debug.Log("Socket task completed!");
    }
}