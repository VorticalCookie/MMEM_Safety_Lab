using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

[System.Serializable]
public class Question
{
    public string questionText;
    public string[] answers;
    public int correctIndex;
    public string explanation;
}

public class QuizManager : MonoBehaviour
{
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI explanationText;
    
    
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

    void Start()
    {
        retryPanel.SetActive(false);
        retryButton.onClick.AddListener(RestartQuiz);
       
    }
    public void StartQuiz()
    {
        quizStarted = true;

        gameObject.SetActive(true); // optional (if quiz panel is disabled)

        currentQuestionIndex = 0;
        correctAnswers = 0;
        isAnswered = false;

        LoadQuestion();
    }

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

        for (int i = 0; i < answerButtons.Length; i++)
        {
            int index = i;

            answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = q.answers[i];
            answerButtons[i].onClick.RemoveAllListeners();
            answerButtons[i].onClick.AddListener(() => AnswerSelected(index));
        }
    }

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

    void TimeUp()
    {
        isAnswered = true;
        explanationText.text = " Time's up!";
        StartCoroutine(NextQuestionDelay());
    }

    IEnumerator NextQuestionDelay()
    {
        yield return new WaitForSeconds(2f);
        currentQuestionIndex++;
        LoadQuestion();
    }


    void RestartQuiz()
    {
        retryPanel.SetActive(false);

        currentQuestionIndex = 0;
        correctAnswers = 0;
        isAnswered = false;
        quizStarted = true;

        LoadQuestion();
    }

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
        }
    }
}