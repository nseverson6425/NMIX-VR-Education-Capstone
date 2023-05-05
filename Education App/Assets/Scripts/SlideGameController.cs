using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SlideGameController : MonoBehaviour
{
    // game stats
    [SerializeField] GameTimer gameTimer;
    [SerializeField] QuestionManager questionManager;
    [SerializeField] QuestionBoard board;
    [SerializeField] ObstacleController obstacleController;
    [SerializeField] Transform gameSpawnPoint;
    [SerializeField] Transform mainSpawnPoint;
    [SerializeField] GameObject player;

    private bool hasObstacle = false;
    private Deck deck;
    private List<string> allAnswers;
    private int score;

    // game timer details
    [SerializeField] float startingTime = 60f;
    private float remainingTime;
    private bool isPaused;

    // advance game
    public bool advanceQuestion = false;
    public bool skipQuestion = false;


    public enum CheckAnswerStatus
    {
        Unchecked, // answer yet to be checked
        Correct, // answer was correct
        Incorrect, // answer was wrong
        AwaitingNextAnswer // answer was correct, waiting for next answer (multi answer question)
    }

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        remainingTime = startingTime;
        isPaused = true; // clock is not counting down
        obstacleController.SetGameController(this);
    }

    public void PrepareGameStart(Deck deck)
    {
        this.deck = deck;
        allAnswers = deck.GetAllAnswers();
        player.transform.position = gameSpawnPoint.transform.position;
    }

    public void StartGame()
    {
        Set questions = deck.NextSet();

        if (questions.questionType == Set.QuestionType.StrictAnswers)
        {
            questionManager.InitializeQuestion(questions.answers, questions.choices, questions.question, deck.GetDeckName());
        }
        else
        {
            questionManager.InitializeQuestion(questions.answers, allAnswers, questions.question, deck.GetDeckName());
        }
        remainingTime = startingTime;
        ResumeTime();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPaused)
        {
            if (remainingTime > 0)
            {
                remainingTime -= Time.deltaTime;
            }
            else
            {
                remainingTime = 0;
            }
            gameTimer.gameTimer.text = remainingTime.ToString("F2");
        }
        
    }


    // starts a wrong answer minigame
    public void TriggerObstacle()
    {
        if (!hasObstacle)
        {
            hasObstacle = true;
            //Debug.Log("trigger obstacle");
            obstacleController.StartDodgeMinigame();
            PauseTime(); // stop counting time
        }
    }

    // add to score
    public void AddScore(int amount)
    {
        // update internal tracker and text display
        score += amount;
        questionManager.UpdateScoreText(score);
    }

    // called by quit game rock
    public void QuitGame()
    {
        Debug.Log("quit game");
        // stop timer
        // stop question
        // stop obstacle
        // respawn player
        player.transform.position = mainSpawnPoint.position;
    }

    // called by skip question rock
    public void SkipQuestion()
    {
        Debug.Log("skip question");
    }

    private void PauseTime()
    {
        isPaused = true;
    }

    private void ResumeTime()
    {
        isPaused = false;
    }

    private void AddTime(float time)
    {
        remainingTime += time;
    }

    public void NextQuestion()
    {
        Set question = deck.NextSet();
        questionManager.ResetDisplays();
        if (question.questionType == Set.QuestionType.StrictAnswers)
        {
            questionManager.InitializeQuestion(question.answers, question.choices, question.question, deck.GetDeckName());
        }
        else
        {
            questionManager.InitializeQuestion(question.answers, allAnswers, question.question, deck.GetDeckName());
        }
    }

    public void ContinueGame()
    {
        hasObstacle = false;
        switch (obstacleController.status)
        {
            case ObstacleController.ObstacleStatus.Passed:
                //passed
                ResumeTime();
                NextQuestion();
                break;
            case ObstacleController.ObstacleStatus.Failed:
                //failed
                ShowPenalty();
                AddTime(10f);
                break;
            default:
                //something broke lol
                ResumeTime();
                NextQuestion();
                break;
        }
    }

    public void ShowPenalty()
    {
        StartCoroutine(PenaltyCoroutine());
    }

    IEnumerator PenaltyCoroutine()
    {
        Color32 orange = new Color32(0xF9, 0x7A, 0x3F, 0xFF);
        Color32 blue = new Color32(0x00, 0xB8, 0xE3, 0xFF);

        //Fade out current text
        yield return StartCoroutine(FadeOutText(board.question));

        //Flash image red 3 times
        for (int i = 0; i < 3; i++)
        {
            board.background.color = orange;
            yield return new WaitForSeconds(0.2f);
            board.background.color = blue;
            yield return new WaitForSeconds(0.2f);
        }

        //Fade in new text
        board.question.text = "Time Penalty! +10s";
        yield return StartCoroutine(FadeInText(board.question));

        //Flash image white and red 3 times
        for (int i = 0; i < 3; i++)
        {
            board.background.color = blue;
            yield return new WaitForSeconds(0.2f);
            board.background.color = orange;
            yield return new WaitForSeconds(0.2f);
        }

        //Fade out text and reset image
        yield return StartCoroutine(FadeOutText(board.question));
        board.question.text = " ";
        board.background.color = blue;
        board.question.CrossFadeAlpha(1, 0f, false);

        ResumeTime();
        NextQuestion();
    }

    IEnumerator FadeOutText(TextMeshProUGUI t)
    {
        t.CrossFadeAlpha(0, 0.5f, false);
        yield return new WaitForSeconds(0.5f);
    }

    IEnumerator FadeInText(TextMeshProUGUI t)
    {
        t.CrossFadeAlpha(1, 0.5f, false);
        yield return new WaitForSeconds(0.5f);
    }
}
