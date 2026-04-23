using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

/// <summary>
/// Clothing Quiz Manager - displays NPC prefabs one at a time and asks
/// "Is this proper work attire?" The player answers Yes or No, sees an
/// explanation, the current NPC is destroyed, and the next one spawns.
/// Made by Marco Espinoza
/// Last Update: 4/22/2026
/// </summary>

[System.Serializable]
public class ClothingQuestion
{
    /// <summary>
    /// The NPC prefab to spawn for this question, whether the outfit is
    /// correct, and an explanation shown after the player answers.
    /// </summary>
    public GameObject npcPrefab;
    public bool isProperAttire;
    public string explanation;
}

public class ClothingQuizManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI questionText;       // Static label: "Is this proper work attire?"
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI explanationText;

    [Header("Answer Buttons")]
    public Button yesButton;
    public Button noButton;

    [Header("Panels")]
    public GameObject retryPanel;
    public Button retryButton;

    [Header("NPC Spawning")]
    /// <summary>
    /// The transform at which each NPC prefab will be instantiated.
    /// Assign an empty GameObject in the scene as the spawn point.
    /// </summary>
    public Transform npcSpawnPoint;

    [Header("Quiz Data")]
    public ClothingQuestion[] questions;
    public float timeLimit = 10f;

    [Header("Events")]
    public UnityEvent onQuizComplete;
    public UnityEvent onPlayerFailed;

    // --- Internal State ---
    private int currentQuestionIndex = 0;
    private int correctAnswers = 0;
    private float timer;
    private bool isAnswered = false;
    private bool quizStarted = false;

    private GameObject currentNPC = null;   // Reference to the live NPC so we can destroy it.

    // --- CSV Logging ---
    private string csvFilePath;

    // -------------------------------------------------------------------------
    // Unity Lifecycle
    // -------------------------------------------------------------------------

    void Start()
    {
        retryPanel.SetActive(false);
        retryButton.onClick.AddListener(RestartQuiz);

        // Wire up the two answer buttons.
        yesButton.onClick.AddListener(() => AnswerSelected(true));
        noButton.onClick.AddListener(()  => AnswerSelected(false));

        // Disable buttons until the quiz is running.
        SetAnswerButtonsInteractable(false);

        // Set up CSV logging.
        csvFilePath = Path.Combine(Application.persistentDataPath, "clothing_quiz_results.csv");
        InitializeCSV();
    }

    void Update()
    {
        if (!quizStarted || isAnswered) return;

        timer -= Time.deltaTime;
        timerText.text = "Time: " + timer.ToString("F1");

        if (timer <= 0f)
        {
            TimeUp();
        }
    }

    // -------------------------------------------------------------------------
    // CSV Logging
    // -------------------------------------------------------------------------

    private void InitializeCSV()
    {
        if (!File.Exists(csvFilePath))
        {
            string header = "QuestionNumber,NPCPrefab,CorrectAnswer,PlayerAnswer,Result,TimeTaken(s)";
            File.WriteAllText(csvFilePath, header + "\n");
            Debug.Log("CSV created at: " + csvFilePath);
        }
    }

    /// <summary>
    /// Appends one row per question to the CSV.
    /// </summary>
    /// <param name="playerAnsweredYes">True if the player pressed Yes, false for No or TimeUp.</param>
    /// <param name="result">"Correct", "Wrong", or "TimeUp"</param>
    private void LogAnswerToCSV(bool playerAnsweredYes, string result)
    {
        float timeTaken = timeLimit - timer;

        ClothingQuestion q = questions[currentQuestionIndex];
        string prefabName = q.npcPrefab != null ? q.npcPrefab.name : "None";
        string correctAnswer = q.isProperAttire ? "Yes" : "No";
        string playerAnswer  = result == "TimeUp" ? "NoAnswer" : (playerAnsweredYes ? "Yes" : "No");

        string row = string.Format("{0},{1},{2},{3},{4},{5:F2}",
            currentQuestionIndex + 1,
            prefabName,
            correctAnswer,
            playerAnswer,
            result,
            timeTaken
        );

        File.AppendAllText(csvFilePath, row + "\n");
    }

    // -------------------------------------------------------------------------
    // Quiz Flow
    // -------------------------------------------------------------------------

    /// <summary>
    /// Call this to begin the quiz from an external script or UnityEvent.
    /// </summary>
    public void StartQuiz()
    {
        quizStarted = true;
        gameObject.SetActive(true);

        currentQuestionIndex = 0;
        correctAnswers = 0;
        isAnswered = false;

        LoadQuestion();
    }

    /// <summary>
    /// Destroys the current NPC (if any), spawns the next one, and resets
    /// the timer and UI for the new question.
    /// </summary>
    void LoadQuestion()
    {
        if (currentQuestionIndex >= questions.Length)
        {
            QuizComplete();
            return;
        }

        ClothingQuestion q = questions[currentQuestionIndex];

        // --- Swap NPC ---
        // Destroy the previous NPC before spawning the new one.
        if (currentNPC != null)
        {
            Destroy(currentNPC);
            currentNPC = null;
        }

        if (q.npcPrefab != null && npcSpawnPoint != null)
        {
            currentNPC = Instantiate(q.npcPrefab, npcSpawnPoint.position, npcSpawnPoint.rotation);
        }
        else
        {
            Debug.LogWarning($"ClothingQuizManager: Question {currentQuestionIndex + 1} is missing " +
                             "an NPC prefab or the spawn point is not assigned.");
        }

        // --- Reset UI ---
        questionText.text = "Is this proper work attire?";
        explanationText.text = "";
        timer = timeLimit;
        isAnswered = false;

        SetAnswerButtonsInteractable(true);
    }

    /// <summary>
    /// Called by the Yes or No button. Evaluates the answer, logs it, and
    /// starts the delay before the next question.
    /// </summary>
    void AnswerSelected(bool playerSaidYes)
    {
        if (isAnswered) return;

        isAnswered = true;
        SetAnswerButtonsInteractable(false);

        ClothingQuestion q = questions[currentQuestionIndex];
        bool isCorrect = (playerSaidYes == q.isProperAttire);

        if (isCorrect)
        {
            correctAnswers++;
            explanationText.text = "✓ Correct!\n" + q.explanation;
            LogAnswerToCSV(playerSaidYes, "Correct");
        }
        else
        {
            explanationText.text = "✗ Incorrect.\n" + q.explanation;
            LogAnswerToCSV(playerSaidYes, "Wrong");
        }

        StartCoroutine(NextQuestionDelay());
    }

    /// <summary>
    /// Called when the timer reaches zero before the player selects an answer.
    /// </summary>
    void TimeUp()
    {
        isAnswered = true;
        SetAnswerButtonsInteractable(false);

        ClothingQuestion q = questions[currentQuestionIndex];
        explanationText.text = "Time's up!\n" + q.explanation;

        LogAnswerToCSV(false, "TimeUp");
        StartCoroutine(NextQuestionDelay());
    }

    /// <summary>
    /// Shows the explanation for a moment, then advances to the next question.
    /// The NPC is destroyed inside LoadQuestion() when the next one spawns,
    /// so the player can still see the current NPC while reading the explanation.
    /// </summary>
    IEnumerator NextQuestionDelay()
    {
        yield return new WaitForSeconds(3f);
        currentQuestionIndex++;
        LoadQuestion();
    }

    /// <summary>
    /// Resets all state and starts from the first question (called by Retry button).
    /// </summary>
    void RestartQuiz()
    {
        retryPanel.SetActive(false);

        // Clean up any lingering NPC from the previous run.
        if (currentNPC != null)
        {
            Destroy(currentNPC);
            currentNPC = null;
        }

        currentQuestionIndex = 0;
        correctAnswers = 0;
        isAnswered = false;
        quizStarted = true;

        LoadQuestion();
    }

    /// <summary>
    /// Called when all questions have been displayed.
    /// Fires onQuizComplete if all were answered correctly; otherwise shows the retry panel.
    /// </summary>
    void QuizComplete()
    {
        // Destroy the last NPC when the quiz ends.
        if (currentNPC != null)
        {
            Destroy(currentNPC);
            currentNPC = null;
        }

        Debug.Log("Clothing Quiz Complete! Results saved to: " + csvFilePath);

        if (correctAnswers == questions.Length)
        {
            Debug.Log("All correct! Great job.");
            onQuizComplete?.Invoke();
        }
        else
        {
            Debug.Log($"Got {correctAnswers}/{questions.Length} correct. Showing retry panel.");
            retryPanel.SetActive(true);
            onPlayerFailed?.Invoke();
            // TODO: Tie into lives system when implemented.
        }
    }

    // -------------------------------------------------------------------------
    // Helpers
    // -------------------------------------------------------------------------

    private void SetAnswerButtonsInteractable(bool state)
    {
        if (yesButton != null) yesButton.interactable = state;
        if (noButton  != null) noButton.interactable  = state;
    }
}
