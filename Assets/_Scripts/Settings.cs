using UnityEngine;

public class Settings : MonoBehaviour
{
    // Call this method from a UI button or event to uncomplete all tasks
    public void UncompleteAllTasks()
    {
        if (TaskManager.Instance != null)
        {
            foreach (var task in TaskManager.Instance.tasks)
            {
                TaskManager.Instance.UncompleteTask(task.taskID);
            }
            Debug.Log("All tasks have been marked as uncompleted.");
        }
        else
        {
            Debug.LogWarning("TaskManager instance not found!");
        }
    }
}
