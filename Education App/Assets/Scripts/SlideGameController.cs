using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SlideGameController : MonoBehaviour
{
    public List<GameObject> answerDisplayScreens = new List<GameObject>(); // references to display screens
    public TextMeshProUGUI questionText; // reference to question display text
    public Transform bombCradleTransform;
    public GameObject projectilePrefab;
    public Image questionDisplayBackground; // background for question display
    public LevelLoader levelLoader; // class for switching between scenes

    // leaderboard stuff
    public GameObject leaderBoard;
    public TextMeshProUGUI leaderBoardScoreText;

    // control canvas references
    public Canvas controlCanvas; // reference to control canvas
    public TextMeshProUGUI controlCanvasGameTimer; // reference to game timer
    public Button nextQuestionButton; // reference to next question button
    public Button skipQuestionButtion; // reference to skip question button
    public Button mainMenuButton; // reference to main menu button
    public Button pauseButton; // reference to pause button
    public Button exitMenuButton; // reference to exit menu button

    // game stat references
    public TextMeshProUGUI gameScoreText; // reference to score on board
    public TextMeshProUGUI studySetNameText; // reference to tile on board
    public TextMeshProUGUI boardGameTimerText;
    public TextMeshProUGUI menuGameTimerText;

    public float timeToWaitForDisplayReset = 1f; // how long to wait from display explosion to reset
    public float timePenaltyInSeconds = 5f;

    private List<Set> sets = new List<Set>(); // list containing all the sets to display in-game
    private List<string> allAnswers = new List<string>(); // list of all answers in sets
    private List<AnswerController> answerControllers = new List<AnswerController>(); // references to display controllers
    private Set currentQuestion; // reference to current displayed question

    // multi answer question variables
    private int numOfSelectedAnswers = 0; // number of answers selected by player

    // question initialization variables
    private List<int> usedQuestions = new List<int>(); // array containing the index of questions that have already been used
    private CheckAnswerStatus questionStatus = CheckAnswerStatus.Unchecked; // status of question

    // score details
    private int playerScore = 0;

    // study set selection variables
    private string studySetName = "Study Set Name";

    // game timer details
    private float elapsedTime = 0.0f;
    private bool isTiming = false;
    public int timeLimitSeconds = 5; // how many seconds the game can go on for


    private GameObject trackedProjectile; // keeps reference to projectile in scene


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

    // hide entirely 
    private void HideControlCanvas()
    {
        controlCanvas.gameObject.SetActive(false); // disable control canvas so its not visible
        mainMenuButton.gameObject.SetActive(false);
        skipQuestionButtion.gameObject.SetActive(false);
        nextQuestionButton.gameObject.SetActive(false);
        controlCanvasGameTimer.gameObject.SetActive(false);
        exitMenuButton.gameObject.SetActive(false);
    }

    // display when question finished and ready for next
    private void DisplayNextQuestionControlCanvas()
    {
        controlCanvas.gameObject.SetActive(true); // enable control canvas so its visible
        mainMenuButton.gameObject.SetActive(false);
        skipQuestionButtion.gameObject.SetActive(false);
        nextQuestionButton.gameObject.SetActive(true);
        controlCanvasGameTimer.gameObject.SetActive(true);
        exitMenuButton.gameObject.SetActive(false);
    }

    // display when pause menu is activated
    private void DisplayControlCanvasAsPause()
    {
        controlCanvas.gameObject.SetActive(true);
        mainMenuButton.gameObject.SetActive(true);
        skipQuestionButtion.gameObject.SetActive(true);
        nextQuestionButton.gameObject.SetActive(false);
        controlCanvasGameTimer.gameObject.SetActive(true);
        exitMenuButton.gameObject.SetActive(true);
    }

    // display at the end of the game
    private void DisplayControlCanvasEndGame()
    {
        controlCanvas.gameObject.SetActive(true);
        mainMenuButton.gameObject.SetActive(true);
        skipQuestionButtion.gameObject.SetActive(false);
        nextQuestionButton.gameObject.SetActive(false);
        controlCanvasGameTimer.gameObject.SetActive(false);
        exitMenuButton.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        //HideControlCanvas(); // make control canvas invisible
        //pauseButton.onClick.AddListener(DisplayControlCanvasAsPause);
        //exitMenuButton.onClick.AddListener(HideControlCanvas);
        nextQuestionButton.onClick.AddListener(WrapUpQuestion);
        skipQuestionButtion.onClick.AddListener(SkipQuestion);
        mainMenuButton.onClick.AddListener(LoadMainMenu);

        leaderBoard.SetActive(false);

        // create sets for the list
        CreateSet("The water cycle is also called the ... ?", CreateAnswerList("Hydrologic Cycle"), Set.QuestionType.MultipleChoice);
        CreateSet("... is the process that turns water vapor into a liquid which causes the formation of a cloud.", CreateAnswerList("Condensation"), Set.QuestionType.MultipleChoice);
        CreateSet("After it rains, the water can either end up on land or ... ?", CreateAnswerList("In a body of water"), Set.QuestionType.MultipleChoice);
        CreateSet("When water evaporates from a leaf, the process is called ... ?", CreateAnswerList("Transpiration"), Set.QuestionType.MultipleChoice);
        CreateSet("When water is heated in an ocean, the liquid water changes form and turns into ... ?", CreateAnswerList("Water Vapor"), Set.QuestionType.MultipleChoice);
        CreateSet("When water leaves a body of water after it is heated, the process is called ... ?", CreateAnswerList("Evaporation"), Set.QuestionType.MultipleChoice);
        CreateSet("When water falls from the sky, the process is known as ... ?", CreateAnswerList("Precipitation"), Set.QuestionType.MultipleChoice);
        CreateSet("When water hits land and is soaked into the ground, the water becomes ... ?", CreateAnswerList("Ground Water"), Set.QuestionType.MultipleChoice);
        /*CreateSet("True or False: The water cycle is a continual process?", CreateAnswerList("True"), Set.QuestionType.TrueOrFalse);
        CreateSet("True or False: Transpiration is a process that occurs on plants and animals?", CreateAnswerList("False"), Set.QuestionType.TrueOrFalse);*/

        answerControllers = new List<AnswerController>();

        // object display controller references
        foreach (GameObject gc in answerDisplayScreens)
        {
            AnswerController ac = gc.GetComponentInChildren<AnswerController>();
            if (ac == null) // check that get component works properly
            {
                Debug.LogError("display has unset answer controller");
            }
            else
            {
                answerControllers.Add(ac);
            }
        }

        // set self reference in display controllers
        foreach (AnswerController ac in answerControllers)
        {
            ac.SetSlideGameControllerReference(this); // set reference
        }

        StartGame("Testing Mode");
    }

    private void LoadMainMenu()
    {
        levelLoader.LoadMainMenu();
    }

    public void StartGame(string studyDeckName)
    {
        // create first question
        Debug.Log("Initialize first question");
        studySetNameText.SetText(studySetName); // set title as study set name
        isTiming = true; // start game counter
        InitializeQuestion();
    }

    // Update is called once per frame
    void Update()
    {
        if (advanceQuestion) // advance question from inspector
        {
            advanceQuestion = false;
            WrapUpQuestion();
        }
        if (skipQuestion)
        {
            skipQuestion = false;
            SkipQuestion();
        }

        // run once question is either correct or false
        if (!(questionStatus == CheckAnswerStatus.Unchecked || questionStatus == CheckAnswerStatus.AwaitingNextAnswer))
        {
            // done with current question
            DisplayNextQuestionControlCanvas(); // provide option for continuing to next question
        }

        if (isTiming)
        {
            elapsedTime += Time.deltaTime;
            SetGameTimers();
        }
    }

    private void SkipQuestion()
    {
        StartCoroutine(TimePenaltyAnimationCorountine());
    }

    private IEnumerator TimePenaltyAnimationCorountine()
    {
        Color32 orange = new Color32(0xF9, 0x7A, 0x3F, 0xFF);
        Color32 blue = new Color32(0x00, 0xB8, 0xE3, 0xFF);


        StartCoroutine(UpdateTextWithFadeCoroutine(questionText, "", 1f)); //clear text
        
        questionDisplayBackground.color = orange; // change to orange
        
        yield return new WaitForSeconds(0.2f);
        questionDisplayBackground.color = blue; // change to blue 

        yield return new WaitForSeconds(0.2f);
        questionDisplayBackground.color = orange; // change to orange

        yield return new WaitForSeconds(0.2f);
        questionDisplayBackground.color = blue; // change to blue

        yield return new WaitForSeconds(0.2f);
        questionDisplayBackground.color = orange; // change to orange

        string timePenaltyText = "TIME PENALTY\n" + timePenaltyInSeconds + " second(s)";
        isTiming = false;

        StartCoroutine(UpdateTextWithFadeCoroutine(questionText, timePenaltyText, 1f)); // display time penalty text

        yield return new WaitForSeconds(3f);
        StartCoroutine(UpdateTextWithFadeCoroutine(questionText, "", 1f)); // clear text

        yield return new WaitForSeconds(1f);
        questionDisplayBackground.color = blue; // change to blue
        elapsedTime += timePenaltyInSeconds;
        isTiming = true;
        WrapUpQuestion();
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

    // handles game time keeping
    private void SetGameTimers()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60.0f);
        int seconds = Mathf.FloorToInt(elapsedTime - minutes * 60);

        //boardGameTimerText.SetText(string.Format("{0:00}:{1:00}", minutes, seconds));
        //menuGameTimerText.SetText(string.Format("{0:00}:{1:00}", minutes, seconds)); 
        boardGameTimerText.SetText(string.Format("Time Elapsed {0}/{1}", seconds, timeLimitSeconds));
        menuGameTimerText.SetText(string.Format("{0}/{1}", seconds, timeLimitSeconds));

        // check for out of time end condition
        if (elapsedTime >= timeLimitSeconds)
        {
            EndGame();
        }
    }

    // make displays blow up
    // reset displays so they can accept new answers/questions
    private void WrapUpQuestion()
    {
        Debug.Log("WrapUpQuestion(): wrapping up question");

        //HideControlCanvas(); // hide control menu
        gameScoreText.SetText(playerScore.ToString());

        foreach (AnswerController ac in answerControllers)
        {
            ac.InitiateDestruction(true);
            //IEnumerator coroutine = WaitAndPrint(timeToWaitForDisplayReset, ac);
            //StartCoroutine(coroutine);
        }
        questionStatus = CheckAnswerStatus.Unchecked; // reset answer check state
        numOfSelectedAnswers = 0; // reset number of selected answers
        InitializeQuestion();
    }

    private void EndGame()
    {
        Debug.Log("EndGame(): Ending game");
        gameScoreText.SetText(playerScore.ToString());

        foreach (AnswerController ac in answerControllers)
        {
            ac.InitiateDestruction(false);
        }
        numOfSelectedAnswers = 0; // reset number of selected answers

        DisplayControlCanvasEndGame(); // display menu for end of game

        StartCoroutine(UpdateTextWithFadeCoroutine(questionText, "GAME OVER\ncheck leaderboard!", 1f));

        //questionText.SetText("GAME OVER");
        isTiming = false;

        string boardScoreText = playerScore + " correct in " + timeLimitSeconds + "s";
        leaderBoard.SetActive(true);
        leaderBoardScoreText.SetText(boardScoreText);
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

    public void SpawnNewProjectile()
    {
        Debug.Log("Spawning projectile");

        Vector3 spawnTransform = new Vector3(bombCradleTransform.position.x,
            bombCradleTransform.position.y + 0.5f,
            bombCradleTransform.position.z);

        // ensure there's only one in the scene
        //if (trackedProjectile == null)
        //{
            // spawn projectile
            trackedProjectile = Instantiate(projectilePrefab, spawnTransform, Quaternion.identity);
        //}
    }

    public void InitializeQuestion()
    {
        numOfSelectedAnswers = 0; // reset count of selected answers
        int selectedIndex = -1;
        bool isValid = false;
        SpawnNewProjectile(); // add projectile to scene

        if (usedQuestions.Count == sets.Count)
        {
            Debug.Log("used all questions in the set, resetting questions");
            usedQuestions.Clear();
        }

        int count = 0;
        // get a valid set index from sets
        while (!isValid)
        {
            count++;
            selectedIndex = Random.Range(0, sets.Count); // random index between 0 and size of sets - 1 (max exclusive)
            if (!usedQuestions.Contains(selectedIndex)) // usedQuestions does NOT contain selected index
            {
                isValid = true;
                usedQuestions.Add(selectedIndex); // add this set to used questions list
            }
            if (count >= 1000)
            {
                break;
            }
        }

        if (selectedIndex <= -1)
        {
            Debug.LogError("something went wrong with randomizing question order");
            selectedIndex = 0;
        }
        currentQuestion = sets[selectedIndex];

        List<string> selectedAnswers = new List<string>();

        if (currentQuestion.questionType == Set.QuestionType.TrueOrFalse)
        {
            selectedAnswers.Add("True");
            selectedAnswers.Add("False");
        }
        else // for multiple choice questions
        {
            //add correct answers
            foreach (string a in currentQuestion.answers)
            {
                selectedAnswers.Add(a); // add answers from answers list in set
            }

            int maxNumOfAnswers = allAnswers.Count - selectedAnswers.Count; // determines number of displays
            if (maxNumOfAnswers >= 4)
            {
                maxNumOfAnswers = 4; // maximum number of displays is 4
            }

            // add incorrect answers
            while (selectedAnswers.Count < maxNumOfAnswers)
            {
                string selectedAnswer = allAnswers[Random.Range(0, allAnswers.Count)];
                if (!(selectedAnswers.Contains(selectedAnswer)))
                {
                    selectedAnswers.Add(selectedAnswer); // add random answer to list of selected answers
                }
            }
        }

        Debug.Log("setting up display positioning and contents");
        // set up displays
        SetDisplayPositions(selectedAnswers);
        SetDisplayContents(selectedAnswers);

        Debug.Log("setting up question text");
        // display question on board
        //questionText.SetText(currentQuestion.question);
        StartCoroutine(UpdateTextWithFadeCoroutine(questionText, currentQuestion.question, 1f));
    }

    // sets the content of the displays
    private void SetDisplayContents(List<string> selectedAnswers)
    {
        int displayControllerIndex = 0; // index of display controller currently being worked on
        while (selectedAnswers.Count > 0)
        {
            string selectedAnswer = selectedAnswers[Random.Range(0, selectedAnswers.Count)];

            selectedAnswers.Remove(selectedAnswer); // remove selected answer from pool of yet to be selected answers
            answerControllers[displayControllerIndex].SetAnswer(selectedAnswer); // send answer to answer controller
            displayControllerIndex++;
        }
    }

    // sets the position of answer displays according to the number of displays being used
    private void SetDisplayPositions(List<string> selectedAnswers)
    {
        GameObject display0 = answerDisplayScreens[0]; // get reference to first display
        GameObject display1 = answerDisplayScreens[1];
        GameObject display2 = answerDisplayScreens[2];
        GameObject display3 = answerDisplayScreens[3];

        Debug.Log("SetDisplayPositions: selected answers count: " + selectedAnswers.Count);

        switch (selectedAnswers.Count)
        {
            case 1:
                // active displays
                display0.SetActive(true);
                display0.transform.position = new Vector3(-3, 1.5f, 0);
                display0.transform.eulerAngles = new Vector3(0, 0, 0);

                // inactive displays
                display1.SetActive(false);
                display2.SetActive(false);
                display3.SetActive(false);
                break;
            case 2:
                // active displays
                display0.SetActive(true);
                display0.transform.position = new Vector3(-3, 1.5f, -2.5f);
                display0.transform.eulerAngles = new Vector3(0, 0, 0);

                display1.SetActive(true);
                display1.transform.position = new Vector3(-3, 1.5f, 2.5f);
                display1.transform.eulerAngles = new Vector3(0, 0, 0);

                // inactive displays
                display2.SetActive(false);
                display3.SetActive(false);
                break;
            case 3:
                // active displays
                display0.SetActive(true);
                display0.transform.position = new Vector3(-1.5f, 1.5f, -4f);
                display0.transform.eulerAngles = new Vector3(0, 315, 0);

                display1.SetActive(true);
                display1.transform.position = new Vector3(-3, 1.5f, 0);
                display1.transform.eulerAngles = new Vector3(0, 0, 0);

                display2.SetActive(true);
                display2.transform.position = new Vector3(-1.5f, 1.5f, 4);
                display2.transform.eulerAngles = new Vector3(0, 45, 0);

                // inactive displays
                display3.SetActive(false);
                break;
            case 4:
                // active displays
                display0.SetActive(true);
                display0.transform.position = new Vector3(-1.5f, 1.5f, -7f);
                display0.transform.eulerAngles = new Vector3(0, 315, 0);

                display1.SetActive(true);
                display1.transform.position = new Vector3(-3, 1.5f, -2.5f);
                display1.transform.eulerAngles = new Vector3(0, 0, 0);

                display2.SetActive(true);
                display2.transform.position = new Vector3(-3, 1.5f, 2.5f);
                display2.transform.eulerAngles = new Vector3(0, 0, 0);

                display3.SetActive(true);
                display3.transform.position = new Vector3(-1.5f, 1.5f, 7);
                display3.transform.eulerAngles = new Vector3(0, 45, 0);
                break;
            default:
                Debug.LogError("something has gone wrong with selecting answers and initializing displays");
                break;
        }
    }

    // TODO make sure that once a display has been selected, it won't check for a right answer again (to avoid checking the same answer twice)
    // TODO add method for changing state of display to show if answer is correct or wrong
    public CheckAnswerStatus CheckAnswer(string answer)
    {
        bool isValid = false;

        foreach (string a in currentQuestion.answers) // check if answer is one of the valid responses
        {
            if (answer.Equals(a))
            {
                isValid = true; // valid answer
                numOfSelectedAnswers++;
                Debug.Log("selected a correct answer");
                break; // exit loop
            }
        }

        // check if all answers have been selected
        if (numOfSelectedAnswers >= currentQuestion.NumOfAnswers())
        {
            Debug.Log("answer is complete");

            // continue to next question
            questionStatus = CheckAnswerStatus.Correct;
            playerScore++; // increment player score
            return CheckAnswerStatus.Correct;
        }
        else if (isValid && numOfSelectedAnswers < currentQuestion.NumOfAnswers())
        {
            Debug.Log("awaiting another correct answer");

            questionStatus = CheckAnswerStatus.AwaitingNextAnswer;
            return CheckAnswerStatus.AwaitingNextAnswer; // need more valid answers
        }
        else
        {
            Debug.Log("selected a wrong answer");

            // continue to next question
            questionStatus = CheckAnswerStatus.Incorrect;
            return CheckAnswerStatus.Incorrect; // wrong answer
        }
    }

    // helper method for quickly creating a list of answers with string input
    private List<string> CreateAnswerList(params string[] answers)
    {
        List<string> answerList = new List<string>();
        foreach (string answer in answers)
        {
            answerList.Add(answer);

            // add elements to all answer list for multiple choice questions
            // check that answer is not "true" or "false" (case insensitive)
            if (!(answer.Equals("True", System.StringComparison.OrdinalIgnoreCase) || answer.Equals("False", System.StringComparison.OrdinalIgnoreCase)))
            {
                allAnswers.Add(answer); // add answer to all answers list
            }
        }

        return answerList;
    }


    // helper method for creating and adding sets to set list
    private void CreateSet(string question, List<string> answers, Set.QuestionType questionType)
    {
        Set newSet = new Set(question, answers, questionType); // create set with params
        sets.Add(newSet); // add to sets
    }
}
