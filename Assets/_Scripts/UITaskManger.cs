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
    }

    [Header("Task UI Elements")]
    public List<TaskUIElement> taskUIElements = new List<TaskUIElement>();

    [Header("Optional Settings")]
    public bool hideCheckmarksOnStart = true;
    public float checkmarkFadeInDuration = 0.3f;

    private void Start()
    {
        // Subscribe to TaskManager events
        if (TaskManager.Instance != null)
        {
            TaskManager.Instance.OnTaskCompleted.AddListener(OnTaskCompleted);
        }
        else
        {
            Debug.LogError("TaskManager instance not found!");
        }

        // Initialize UI - hide checkmarks if needed
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

        // Check for already completed tasks (in case UI is loaded after tasks are done)
        RefreshUI();
    }

    private void OnDestroy()
    {
        // Unsubscribe from events
        if (TaskManager.Instance != null)
        {
            TaskManager.Instance.OnTaskCompleted.RemoveListener(OnTaskCompleted);
        }
    }

    /// <summary>
    /// Called when a task is completed - updates the corresponding UI element.
    /// </summary>
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

    /// <summary>
    /// Updates a single task UI element (checkmark and text).
    /// </summary>
    private void UpdateTaskUI(TaskUIElement uiElement, bool animate = false)
    {
        // Update text FIRST (so it's visible immediately)
        if (uiElement.taskText != null)
        {
            uiElement.taskText.text = uiElement.completedText;
            uiElement.taskText.color = uiElement.completedTextColor;
        }

        // Enable and show checkmark
        if (uiElement.checkmarkImage != null)
        {
            uiElement.checkmarkImage.enabled = true;

            if (animate && checkmarkFadeInDuration > 0)
            {
                StartCoroutine(FadeInCheckmark(uiElement.checkmarkImage));
            }
        }
    }

    /// <summary>
    /// Refreshes all UI elements based on current task completion status.
    /// Useful for initializing UI or syncing after loading.
    /// </summary>
    public void RefreshUI()
    {
        if (TaskManager.Instance == null) return;

        foreach (var uiElement in taskUIElements)
        {
            bool isCompleted = TaskManager.Instance.IsTaskCompleted(uiElement.taskID);

            if (isCompleted)
            {
                UpdateTaskUI(uiElement, false); // No animation on refresh
            }
        }
    }

    /// <summary>
    /// Optional fade-in animation for checkmark.
    /// </summary>
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
            TaskManager.Instance.OnTaskCompleted.AddListener(OnTaskCompleted);
    }

    private void OnDisable()
    {
        if (TaskManager.Instance != null)
            TaskManager.Instance.OnTaskCompleted.RemoveListener(OnTaskCompleted);
    }
}