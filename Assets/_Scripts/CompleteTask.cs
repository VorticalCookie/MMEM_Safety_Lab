using UnityEngine;

public class CompleteTask : MonoBehaviour
{
    public string taskID;

    public void Complete()
    {
        if (TaskManager.Instance != null)
        {
            TaskManager.Instance.CompleteTask(taskID);
        }
    }
}
