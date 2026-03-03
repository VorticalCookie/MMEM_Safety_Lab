using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// UITaskManager handles the visual representation of tasks in the UI.
/// Shows checkmarks and updates text when tasks are completed.
/// Made by Marco Espinoza (edited by Claude)
/// Last Update: 2/9/2026
/// </summary>
public class UITaskManager : MonoBehaviour
{
    [System.Serializable]
    public class TaskUIElement
    {
        public string taskID;
        public Image checkmarkImage;
        public TextMeshProUGUI taskText;
        [TextArea(2, 4)]
        public string completedText = "✓ Task Completed!";
        public Color completedTextColor = Color.green;
        [TextArea(2, 4)]
        public string incompleteText = "Task not completed."; // Add this for incomplete state
        public Color incompleteTextColor = Color.white;
    }

    public List<TaskUIElement> taskUIElements = new List<TaskUIElement>();
    public bool hideCheckmarksOnStart = true;
    public float checkmarkFadeInDuration = 0.3f;

    private void Start()
    {
        if (TaskManager.Instance != null)
        {
            TaskManager.Instance.OnTaskCompleted.AddListener(OnTaskCompleted);
            TaskManager.Instance.OnTaskUncompleted.AddListener(OnTaskUncompleted); // Subscribe
        }
        else
        {
            Debug.LogError("TaskManager instance not found!");
        }

        if (hideCheckmarksOnStart)
        {
            foreach (var uiElement in taskUIElements)
            {
                if (uiElement.checkmarkImage != null)
                {
                    uiElement.checkmarkImage.enabled = false;
                }
            }
        }

        RefreshUI();
    }

    private void OnDestroy()
    {
        if (TaskManager.Instance != null)
        {
            TaskManager.Instance.OnTaskCompleted.RemoveListener(OnTaskCompleted);
            TaskManager.Instance.OnTaskUncompleted.RemoveListener(OnTaskUncompleted); // Unsubscribe
        }
    }

    private void OnTaskCompleted(Task completedTask)
    {
        TaskUIElement uiElement = taskUIElements.Find(ui => ui.taskID == completedTask.taskID);
        if (uiElement == null)
        {
            Debug.LogWarning($"No UI element found for task ID: {completedTask.taskID}");
            return;
        }
        UpdateTaskUI(uiElement, true);
    }

    private void OnTaskUncompleted(Task uncompletedTask)
    {
        TaskUIElement uiElement = taskUIElements.Find(ui => ui.taskID == uncompletedTask.taskID);
        if (uiElement == null)
        {
            Debug.LogWarning($"No UI element found for task ID: {uncompletedTask.taskID}");
            return;
        }
        ResetTaskUI(uiElement);
    }

    private void UpdateTaskUI(TaskUIElement uiElement, bool animate = false)
    {
        if (uiElement.taskText != null)
        {
            uiElement.taskText.text = uiElement.completedText;
            uiElement.taskText.color = uiElement.completedTextColor;
        }
        if (uiElement.checkmarkImage != null)
        {
            uiElement.checkmarkImage.enabled = true;
            if (animate && checkmarkFadeInDuration > 0)
            {
                StartCoroutine(FadeInCheckmark(uiElement.checkmarkImage));
            }
        }
    }

    private void ResetTaskUI(TaskUIElement uiElement)
    {
        if (uiElement.taskText != null)
        {
            uiElement.taskText.text = uiElement.incompleteText;
            uiElement.taskText.color = uiElement.incompleteTextColor;
        }
        if (uiElement.checkmarkImage != null)
        {
            uiElement.checkmarkImage.enabled = false;
        }
    }

    public void RefreshUI()
    {
        if (TaskManager.Instance == null) return;
        foreach (var uiElement in taskUIElements)
        {
            bool isCompleted = TaskManager.Instance.IsTaskCompleted(uiElement.taskID);
            if (isCompleted)
            {
                UpdateTaskUI(uiElement, false);
            }
            else
            {
                ResetTaskUI(uiElement);
            }
        }
    }

    private System.Collections.IEnumerator FadeInCheckmark(Image checkmark)
    {
        Color originalColor = checkmark.color;
        Color transparent = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
        checkmark.color = transparent;
        float elapsed = 0f;
        while (elapsed < checkmarkFadeInDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, originalColor.a, elapsed / checkmarkFadeInDuration);
            checkmark.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }
        checkmark.color = originalColor;
    }

    private void OnEnable()
    {
        if (TaskManager.Instance != null)
        {
            TaskManager.Instance.OnTaskCompleted.AddListener(OnTaskCompleted);
            TaskManager.Instance.OnTaskUncompleted.AddListener(OnTaskUncompleted);
        }
    }

    private void OnDisable()
    {
        if (TaskManager.Instance != null)
        {
            TaskManager.Instance.OnTaskCompleted.RemoveListener(OnTaskCompleted);
            TaskManager.Instance.OnTaskUncompleted.RemoveListener(OnTaskUncompleted);
        }
    }
}
