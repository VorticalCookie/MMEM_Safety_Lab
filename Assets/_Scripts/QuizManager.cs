using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

/// <summary>
/// Quiz Manager to handle quiz questions, answers, timer, and retry logic.
/// Made by Marco Espinoza
/// Last Update: 4/10/2026
/// </summary>
/// 

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

    public UnityEvent onQuizComplete;
    public UnityEvent OnPlayerFailed;

    //Hide retry panel on start and set up retry button listener
    void Start()
    {
        retryPanel.SetActive(false);
        retryButton.onClick.AddListener(RestartQuiz);
       
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
    /// Updates the timer for the current question. If the timer runs out, it calls the TimeUp method to handle the timeout scenario.
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

    /// Loads the current question and sets up the answer buttons. It also resets the timer for the new question.
    void LoadQuestion()
    {
        if (currentQuestionIndex >= questions.Length)
        {
            QuizComplete();
            return;
        }

        /// Load the current question and update the UI .
        Question q = questions[currentQuestionIndex];
        /// Set the question text and reset the explanation text. Also, reset the timer and answer state for the new question.
        questionText.text = q.questionText;
        explanationText.text = "";
        timer = timeLimit;
        isAnswered = false;

        /// Set up answer buttons
        for (int i = 0; i < answerButtons.Length; i++)
        {
            int index = i;

            answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = q.answers[i];
            answerButtons[i].onClick.RemoveAllListeners();
            answerButtons[i].onClick.AddListener(() => AnswerSelected(index));
        }
    }
    /// <summary>
    /// Handles the logic when an answer is selected. It checks if the selected answer is correct, updates the score, and displays the explanation.
    /// It also starts a delay before loading the next question.
    /// </summary>
    /// <param name="index"></param>
    void AnswerSelected(int index)
    {
        if (isAnswered) return;

        isAnswered = true;

        Question q = questions[currentQuestionIndex];

        if (index == q.correctIndex)
        {
            correctAnswers++;
            explanationText.text = "v/ " + q.explanation;
        }
        else
        {
            explanationText.text = "X Wrong answer.";
        }

        StartCoroutine(NextQuestionDelay());
    }

    /// Handles the scenario when the timer runs out before an answer is selected.
    /// It marks the question as answered, displays a timeout message, and starts a delay before loading the next question.
    void TimeUp()
    {
        isAnswered = true;
        explanationText.text = " Time's up!";
        StartCoroutine(NextQuestionDelay());
    }

    /// Waits for a short delay before loading the next question. 
    /// This allows the player to see the explanation or timeout message before moving on.
    IEnumerator NextQuestionDelay()
    {
        yield return new WaitForSeconds(3f);
        currentQuestionIndex++;
        LoadQuestion();
    }

    /// Resets the quiz state and starts the quiz again. 
    /// This method is called when the player clicks the retry button after failing the quiz.
    void RestartQuiz()
    {
        retryPanel.SetActive(false);

        currentQuestionIndex = 0;
        correctAnswers = 0;
        isAnswered = false;
        quizStarted = true;

        LoadQuestion();
    }

    /// Called when the quiz is completed.
    /// It checks if all answers were correct and either invokes the completion event or shows the retry panel.
    void QuizComplete()
    {
        Debug.Log("Quiz Complete!");

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
            //Add lives system and reduce 1 live in the future
        }
    }

    //Note to future self: Finish CSV before sprint 7
    //https://docs.unity3d.com/Packages/com.unity.localization@1.2/manual/CSV.html
    //https://www.youtube.com/watch?v=tI9NEm02EuE
    //Currently waiting for the finalized test that is going to be taken by the focus group and the vr group to determine the final questions and answers for the final quiz.
    //Once we have that, I will implement the CSV loading functionality to make it easier to manage the quiz content.
}