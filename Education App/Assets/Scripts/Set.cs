using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Set
{
    public string question; // question for the answers in this set
    // list containing answers relevant to this question, normally 1 but if multiple answers provide that choice
    public List<string> answers;
    public List<string> choices; // answers for strict questions
    public QuestionType questionType;
    
    public enum QuestionType
    {
        MultipleChoice,
        TrueOrFalse,
        StrictAnswers,
        Undefined
    }

    public Set()
    {
        question = "Question not set";
        answers = null;
        questionType = QuestionType.Undefined;
    }

    // contructor
    public Set(string question, List<string> answers, QuestionType questionType)
    {
        this.question = question;
        this.answers = answers;
        this.questionType = questionType;
    }
    public Set(string question, List<string> answers, List<string> choices, QuestionType questionType)
    {
        this.question = question;
        this.answers = answers;
        this.questionType = questionType;
        this.choices = choices;
    }


    // print data in this set
    public void PrintSet()
    {
        Debug.Log(question);
        Debug.Log(answers);
    }

    // return the number of answers valid for this question
    public int NumOfAnswers()
    {
        return answers.Count;
    }
}
