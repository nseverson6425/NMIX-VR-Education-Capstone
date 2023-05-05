using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestionManager : MonoBehaviour
{
    [SerializeField] QuestionBoard questionBoard;
    [SerializeField] List<AnswerChoice> answerChoices;
    [SerializeField] SlideGameController gameManager;
    [SerializeField] int questionScoreValue = 5;

    private List<string> correctAnswers;
    private List<string> choices;
    private string question;
    private string deckName;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitializeQuestion(List<string> correctAnswers, List<string> choices, string question, string deckName)
    {
        ResetDisplays();
        this.correctAnswers = correctAnswers;
        this.choices = choices;
        this.question = question;
        this.deckName = deckName;

        SetQuestion(); // update question screen
        SetAnswers(); // update rock displays
    }

    public void CheckAnswer(string answer)
    {
        List<string> checkAnswers = new List<string>(correctAnswers); // make copy of list
        bool isFound = checkAnswers.Remove(answer);

        if (isFound) // correct
        {
            if (checkAnswers.Count <= 0) // found all answers
            {
                // add points
                gameManager.AddScore(questionScoreValue);
                // reset displays
                //ResetDisplays(); // done checking or answers
                gameManager.NextQuestion();
            }

        }
        else // wrong answer
        {
            ResetDisplays(); // need to be cleared for next question anyways
            questionBoard.question.text = "Incorrect Answer!";
            // trigger game obstacle
            gameManager.TriggerObstacle();
        }
    }

    public void SetQuestion()
    {
        questionBoard.question.text = question;
        questionBoard.deckName.text = deckName;
    }

    // TODO does not account for T/F questions
    public void SetAnswers()
    {
        List<string> output = new List<string>();

        // Add correct answers to output list
        foreach (string answer in correctAnswers)
        {
            Debug.Log("SetAnswers(): answer - " + answer);
            output.Add(answer);
        }

        // account for less than 4 choices
        // If correct answers don't fill the output list, add choices
        while ((output.Count < 4) && ((output.Count - correctAnswers.Count) < choices.Count))
        {
            int randomIndex = Random.Range(0, choices.Count);
            string randomChoice = choices[randomIndex];

            // Only add choice if it's not already in the output list
            if (!output.Contains(randomChoice) && !correctAnswers.Contains(randomChoice))
            {
                output.Add(randomChoice);
            }
        }

        int index = 0;
        // Shuffle the output list
        output = output.OrderBy(x => Random.value).ToList();

        foreach (AnswerChoice ac in answerChoices)
        {
            ac.choice.text = output[index];
            index++;
        }
    }

    public void ResetDisplays()
    {
        //Debug.Log("reset display");
        foreach (AnswerChoice ac in answerChoices)
        {
            ac.ResetChoice();
        }
    }

    public void UpdateScoreText(int score)
    {
        questionBoard.score.text = score.ToString();
    }
}
