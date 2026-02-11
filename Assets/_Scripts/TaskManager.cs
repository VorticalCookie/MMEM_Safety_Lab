using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// TaskManager to check complete status of different tasks to check progress of the material learned.
/// Made by Marco Espinoza
/// Last Update: 2/9/2026
/// </summary>

public class TaskManager : MonoBehaviour
{
    // Singleton instance
    public static TaskManager Instance;

    // List of all tasks in the game
    public List<Task> tasks = new List<Task>();

    // Events for completed Tasks
    public UnityEvent<Task> OnTaskCompleted;
    public UnityEvent OnAllTasksCompleted;

    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    /// <summary>
    /// Marks a task as completed and checks if all required tasks are completed.
    /// </summary>
    public void CompleteTask(string taskID)
    {
        
        Task task = tasks.Find(t => t.taskID == taskID);

        if (task == null)
        {
            Debug.LogWarning($"Task with ID {taskID} not found.");
            return;
        }

        if (task.isCompleted)
            return;

        task.isCompleted = true;
        Debug.Log($"Task completed: {task.description}");

        OnTaskCompleted?.Invoke(task);


        if (RequiredTasksCompleted())
        {
            Debug.Log("All required tasks completed!");
            OnAllTasksCompleted?.Invoke();
        }
    }

    /// <summary>
    /// Checks if all required tasks are completed. We still need to discuss with Jeff and Andy(the supervisor for PUMAS) what happens when all task are complete
    /// </summary>
    public bool RequiredTasksCompleted()
    {
        foreach (Task task in tasks)
        {
            if (!task.isCompleted)
                return false;
        }
        return true;
    }

    /// <summary>
    /// Checks if a specific task is completed.
    /// </summary>
   
    public bool IsTaskCompleted(string taskID)
    {
        Task task = tasks.Find(t => t.taskID == taskID);
        return task != null && task.isCompleted;
    }
}

