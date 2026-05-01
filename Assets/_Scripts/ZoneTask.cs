using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Completes a task when the player stays inside a trigger zone for a specified amount of time.
/// Made by Marco Espinoza
/// Last Update: 2/23/2026
/// Chase if you're reading this and I'm not here in class, can you send me the notes, I have a cold and I don't want to get anyone else sick. Thanks!
/// </summary>
/// 


public class ZoneTask : MonoBehaviour
{
    /// <summary>
    /// When the player stays inside the trigger zone for the required time, the task will be marked as completed in the TaskManager.
    /// </summary>
    [Header("Task Settings")]
    public string taskID;
    public float requiredTime = 3f; // Time the player must stay inside the zone

    [Header("Particle Effects")]
    public ParticleSystem particleEffects; // Particle effect to play when the task is completed


    [Header("Events")]
    public UnityEvent OnPlayerEnterZone; // Optional event for when player comes close
    public UnityEvent OnPlayerExitZone; // Optional event for when player comes close


    private float timer = 0f;
    private bool playerInside = false;
    private bool taskCompleted = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !taskCompleted)
        {
            playerInside = true;
            timer = 0f;
            OnPlayerEnterZone?.Invoke();

            if (particleEffects != null && !particleEffects.isPlaying)
            {
                particleEffects.Play();
            }
        }
    }

    /// <summary>
    /// If the player exits the trigger zone before the required time, the timer resets and the task will not be completed. 
    /// We can use this to also check when the players don't spend enough time inside a lab, but we might want to keep their time later on
    ///
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            timer = 0f;

           
            OnPlayerExitZone?.Invoke();

            if (particleEffects != null && particleEffects.isPlaying)
            {
                particleEffects.Stop();
            }

        }
    }

    private void Update()
    {
        if (playerInside && !taskCompleted)
        {
            timer += Time.deltaTime;
            if (timer >= requiredTime)
            {
                CompleteTask();
            }
        }
    }
    /// <summary>
    /// Marks the task as completed in the TaskManager and talks with the TaskUIManager.
    /// </summary>
    private void CompleteTask()
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
}
