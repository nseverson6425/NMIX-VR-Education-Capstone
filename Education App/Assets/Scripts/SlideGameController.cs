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
    [SerializeField] ObstacleController obstacleController;
    [SerializeField] Transform gameSpawnPoint;
    [SerializeField] GameObject player;

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
        questionManager.InitializeQuestion(questions.answers, allAnswers, questions.question, deck.GetDeckName());
        remainingTime = startingTime;
        ResumeTime();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPaused && remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
        }
        else
        {
            remainingTime = 0;
        }
        gameTimer.gameTimer.text = remainingTime.ToString("F2");
    }

    private IEnumerator TimePenaltyAnimationCorountine()
    {
        Color32 orange = new Color32(0xF9, 0x7A, 0x3F, 0xFF);
        Color32 blue = new Color32(0x00, 0xB8, 0xE3, 0xFF);


        //StartCoroutine(UpdateTextWithFadeCoroutine(questionText, "", 1f)); //clear text
        
        //questionDisplayBackground.color = orange; // change to orange
        
        yield return new WaitForSeconds(0.2f);
        //questionDisplayBackground.color = blue; // change to blue 

        yield return new WaitForSeconds(0.2f);
        //questionDisplayBackground.color = orange; // change to orange

        yield return new WaitForSeconds(0.2f);
        //questionDisplayBackground.color = blue; // change to blue

        yield return new WaitForSeconds(0.2f);
        //questionDisplayBackground.color = orange; // change to orange

        //string timePenaltyText = "TIME PENALTY\n" + timePenaltyInSeconds + " second(s)";

        //StartCoroutine(UpdateTextWithFadeCoroutine(questionText, timePenaltyText, 1f)); // display time penalty text

        yield return new WaitForSeconds(3f);
        //StartCoroutine(UpdateTextWithFadeCoroutine(questionText, "", 1f)); // clear text

        yield return new WaitForSeconds(1f);
        //questionDisplayBackground.color = blue; // change to blue
        //elapsedTime += timePenaltyInSeconds;
    }

    private IEnumerator UpdateTextWithFadeCoroutine(TextMeshProUGUI textUI, string newText, float fadeTime)
    {
        Debug.Log("fading out text");
        StartCoroutine(FadeTextToZeroAlpha(fadeTime * 0.5f, textUI)); //fade out
        yield return new WaitForSeconds(fadeTime * 0.6f); //wait for fade out
        Debug.Log("replacing and fading in text");
        textUI.SetText(newText); //replace text
        StartCoroutine(FadeTextToFullAlpha(fadeTime * 0.5f, textUI)); // fade in
    }

    public IEnumerator FadeTextToFullAlpha(float time, TextMeshProUGUI textToFadeIn)
    {
        textToFadeIn.color = new Color(textToFadeIn.color.r, textToFadeIn.color.g, textToFadeIn.color.b, 0);
        while (textToFadeIn.color.a < 1.0f)
        {
            textToFadeIn.color = new Color(textToFadeIn.color.r, textToFadeIn.color.g, textToFadeIn.color.b, textToFadeIn.color.a + (Time.deltaTime / time));
            yield return null;
        }
    }

    public IEnumerator FadeTextToZeroAlpha(float time, TextMeshProUGUI textToFadeOut)
    {
        textToFadeOut.color = new Color(textToFadeOut.color.r, textToFadeOut.color.g, textToFadeOut.color.b, 1);
        while (textToFadeOut.color.a > 0.0f)
        {
            textToFadeOut.color = new Color(textToFadeOut.color.r, textToFadeOut.color.g, textToFadeOut.color.b, textToFadeOut.color.a - (Time.deltaTime / time));
            yield return null;
        }
    }

    private IEnumerator WaitAndPrint(float waitTime, AnswerController ac)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            Debug.Log("Reseting display");
            ac.ResetDisplay();
        }
    }

    // starts a wrong answer minigame
    public void TriggerObstacle()
    {
        Debug.Log("trigger obstacle");
        PauseTime(); // stop counting time
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

    }

    // called by skip question rock
    public void SkipQuestion()
    {
        StartCoroutine(TimePenaltyAnimationCorountine());
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
        questionManager.InitializeQuestion(question.answers, allAnswers, question.question, deck.GetDeckName());
    }

    public void ContinueGame()
    {
        switch (obstacleController.status)
        {
            case ObstacleController.ObstacleStatus.Passed:
                //passed
                break;
            case ObstacleController.ObstacleStatus.Failed:
                //failed
                break;
            default:
                //something broke lol
                break;
        }
        ResumeTime();
        NextQuestion();
    }
}
