using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;


/// <summary>
/// Completes a task when the player stays clears the area from any trash
/// Made by Marco Espinoza
/// Last Update: 2/26/2026
/// </summary>
/// 


public class WhiteZone : MonoBehaviour
{

    [Header("Task Settings")]
    public string taskID;

    [Header("Events")]
    public UnityEvent TrashDetected;

    [Header("Detection Settings")]
    public string tagToDetect = "Trash";

    // Track trash objects inside the zone
    private HashSet<GameObject> trashInZone = new HashSet<GameObject>();
    private bool taskCompleted = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(tagToDetect))
        {
            trashInZone.Add(other.gameObject);
            print("Trash entered the zone, task marked as incomplete.");
            if (taskCompleted)
            {
                MarkTaskIncomplete();
            }
            TrashDetected?.Invoke(); // <-- Invoke the event here
            
        }
    }

    /// When trash exits the zone, we check if there is any trash left. If not, we mark the task as complete.
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(tagToDetect))
        {

            print("Trash exit the zone, task marked as complete.");


            trashInZone.Remove(other.gameObject);
            // If no trash left, clear zone and complete task
            if (trashInZone.Count == 0)
            {
               
                MarkTaskComplete();
            }
        }
    }


    /// Marks the task as complete in the TaskManager when there is no trash left in the zone.
    private void MarkTaskComplete()
    {
        taskCompleted = true;
        if (TaskManager.Instance != null)
        {
            TaskManager.Instance.CompleteTask(taskID);
        }
        else
        {
            Debug.LogWarning("TaskManager instance not found!");
        }
    }
    /// Marks the task as incomplete in the TaskManager when trash enters the zone.
    private void MarkTaskIncomplete()
    {
        taskCompleted = false;
        if (TaskManager.Instance != null)
        {
            TaskManager.Instance.UncompleteTask(taskID);
        }
        else
        {
            Debug.LogWarning("TaskManager instance not found!");
        }
    }

}
