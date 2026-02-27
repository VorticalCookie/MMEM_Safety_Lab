using UnityEngine;
using System.Collections.Generic;


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

    // Track trash objects inside the zone
    private HashSet<GameObject> trashInZone = new HashSet<GameObject>();
    private bool taskCompleted = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Trash"))
        {
            trashInZone.Add(other.gameObject);
            print("Trash entered the zone, task marked as incomplete.");
            if (taskCompleted)
            {
                MarkTaskIncomplete();
                
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Trash"))
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
