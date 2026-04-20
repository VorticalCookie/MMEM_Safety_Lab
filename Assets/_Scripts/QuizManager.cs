using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

/// <summary>
/// Quiz Manager to handle quiz questions, answers, timer, and retry logic.
/// Made by Marco Espinoza
/// Last Update: 4/20/2026
/// </summary>

[System.Serializable]
public class Question
{
    /// <summary>
    /// The text of the question, the possible answers, the index of the correct answer, and an explanation for the correct answer.
    /// </summary>
    public string questionText;
    public string[] answers;
    public int correctIndex;
    public string explanation;
}

public class QuizManager : MonoBehaviour
{
    /// UI references for displaying the question, timer, explanation, and retry panel.
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI explanationText;

    /// Retry panel and button to allow the player to retry the quiz if they fail.
    public GameObject retryPanel;
    public Button retryButton;

    public Button[] answerButtons;
    public Question[] questions;
    public float timeLimit = 5f;

    private int currentQuestionIndex = 0;
    private int correctAnswers = 0;
    private float timer;
    private bool isAnswered = false;
    private bool quizStarted = false;

    // --- CSV Logging ---
    // Tracks when the current question was displayed, so we can calculate time taken.
    private float questionStartTime;

    // Full path to the CSV file. C:\Users\marco\AppData\LocalLow\CSU Chico\MMEM_SafetyLab
    private string csvFilePath;

    public UnityEvent onQuizComplete;
    public UnityEvent OnPlayerFailed;

    // Hide retry panel on start and set up retry button listener.
    void Start()
    {
        retryPanel.SetActive(false);
        retryButton.onClick.AddListener(RestartQuiz);

        // Build the CSV path and write the header row if the file doesn't exist yet.
        csvFilePath = Path.Combine(Application.persistentDataPath, "quiz_results.csv");
        InitializeCSV();
    }

    /// <summary>
    /// Creates the CSV file and writes the header row on first run.
    /// If the file already exists (e.g. from a previous session) the header is not duplicated.
    /// </summary>
    private void InitializeCSV()
    {
        if (!File.Exists(csvFilePath))
        {
            // Header row — edit column names here if needed.
            string header = "QuestionNumber,QuestionText,SelectedResult,TimeTaken(s)";
            File.WriteAllText(csvFilePath, header + "\n");
            Debug.Log("CSV created at: " + csvFilePath);
        }
    }

    /// <summary>
    /// Appends one row to the CSV for the question that was just answered.
    /// </summary>
    /// <param name="result">Human-readable outcome: "Correct", "Wrong", or "TimeUp"</param>
    private void LogAnswerToCSV(string result)
    {
        float timeTaken = timeLimit - timer; // How many seconds elapsed before answering.

        // Sanitize the question text: wrap in quotes and escape any internal quotes
        // so commas or quotes in the question don't break the CSV format.
        string safeQuestion = "\"" + questions[currentQuestionIndex].questionText.Replace("\"", "\"\"") + "\"";

        string row = string.Format("{0},{1},{2},{3:F2}",
            currentQuestionIndex + 1,   // 1-based question number
            safeQuestion,               // question text (quoted)
            result,                     // Correct / Wrong / TimeUp
            timeTaken                   // seconds taken, 2 decimal places
        );

        File.AppendAllText(csvFilePath, row + "\n");
    }

    /// <summary>
    /// Call this method to start the quiz. It initializes the quiz state and loads the first question.
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
    /// Updates the timer for the current question. If the timer runs out, it calls TimeUp().
    /// </summary>
    void Update()
    {
        if (!quizStarted || isAnswered) return;

        timer -= Time.deltaTime;
        timerText.text = "Time: " + timer.ToString("F1");

        if (timer <= 0)
        {
            TimeUp();
        }
    }

    /// Loads the current question and sets up the answer buttons. Also resets the timer.
    void LoadQuestion()
    {
        if (currentQuestionIndex >= questions.Length)
        {
            QuizComplete();
            return;
        }

        Question q = questions[currentQuestionIndex];

        questionText.text = q.questionText;
        explanationText.text = "";
        timer = timeLimit;
        isAnswered = false;

        // Record the moment this question became active so we can compute elapsed time later.
        questionStartTime = Time.time;

        // Set up answer buttons.
        for (int i = 0; i < answerButtons.Length; i++)
        {
            int index = i;
            answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = q.answers[i];
            answerButtons[i].onClick.RemoveAllListeners();
            answerButtons[i].onClick.AddListener(() => AnswerSelected(index));
        }
    }

    /// <summary>
    /// Handles the logic when an answer is selected.
    /// Checks correctness, updates score, logs result to CSV, and shows the explanation.
    /// </summary>
    void AnswerSelected(int index)
    {
        if (isAnswered) return;

        isAnswered = true;

        Question q = questions[currentQuestionIndex];

        if (index == q.correctIndex)
        {
            correctAnswers++;
            explanationText.text = "v/ " + q.explanation;
            LogAnswerToCSV("Correct");
        }
        else
        {
            explanationText.text = "X Wrong answer.";
            LogAnswerToCSV("Wrong");
        }

        StartCoroutine(NextQuestionDelay());
    }

    /// Handles the scenario when the timer runs out before an answer is selected.
    void TimeUp()
    {
        isAnswered = true;
        explanationText.text = "Time's up!";
        LogAnswerToCSV("TimeUp");
        StartCoroutine(NextQuestionDelay());
    }

    /// Waits briefly so the player can read the explanation, then advances to the next question.
    IEnumerator NextQuestionDelay()
    {
        yield return new WaitForSeconds(3f);
        currentQuestionIndex++;
        LoadQuestion();
    }

    /// Resets the quiz state and starts over (called by the retry button).
    void RestartQuiz()
    {
        retryPanel.SetActive(false);

        currentQuestionIndex = 0;
        correctAnswers = 0;
        isAnswered = false;
        quizStarted = true;

        LoadQuestion();
    }

    /// Called when all questions have been shown.
    /// Passes if all answers were correct; shows retry panel otherwise.
    void QuizComplete()
    {
        Debug.Log("Quiz Complete! Results saved to: " + csvFilePath);

        if (correctAnswers == questions.Length)
        {
            Debug.Log("All correct!");
            onQuizComplete.Invoke();
        }
        else
        {
            Debug.Log("Not all correct. Show retry.");
            retryPanel.SetActive(true);
            OnPlayerFailed?.Invoke();
            // TODO: Add lives system and reduce 1 life.
        }
    }

    // Note to future self: Finish CSV import before sprint 7
    // https://docs.unity3d.com/Packages/com.unity.localization@1.2/manual/CSV.html
    // https://www.youtube.com/watch?v=tI9NEm02EuE
    // Currently waiting for the finalized test from the focus group and VR group
    // to determine final questions and answers.
    // Once we have that, implement CSV loading to manage quiz content externally.
}