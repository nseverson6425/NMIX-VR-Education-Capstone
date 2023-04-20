using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck
{
    private List<Set> deck;
    private int index;
    private string deckName;

    public Deck()
    {
        index = 0;
        deckName = "Unnamed";
        deck = new List<Set>();
    }

    public Deck(List<Set> sets, string deckName)
    {
        this.deckName = deckName;
        index = 0;
        deck = sets;
    }

    public void AddSet(string question, List<string> answerList, Set.QuestionType type)
    {
        Set newSet = new Set(question, answerList, type); // create set with params
        deck.Add(newSet); // add to sets
    }

    public List<string> CreateAnswerList(params string[] answers)
    {
        List<string> answerList = new List<string>();
        foreach (string answer in answers)
        {
            answerList.Add(answer);
        }

        return answerList;
    }

    // empties deck
    public void ClearDeck()
    {
        index = 0;
        deck.Clear();
    }

    public Set NextSet()
    {
        Set set;
        if (index >= deck.Count)
        {
            index = 0;
            set = deck[index];
            index++;

            return set;
        } 
        else if (deck.Count <= 0)
        {
            Debug.LogError("Set is empty");
            return new Set();
        }
        else
        {
            set = deck[index];
            index++;

            return set;
        }
    }

    // returns all the answers contained in this deck (excluding true and false)
    public List<string> GetAllAnswers()
    {
        List<string> allAnswers = new List<string>();

        foreach (Set set in deck)
        {
            foreach(string answer in set.answers)
            {
                if (!(answer.Equals("True", System.StringComparison.OrdinalIgnoreCase) || answer.Equals("False", System.StringComparison.OrdinalIgnoreCase)))
                {
                    allAnswers.Add(answer); // add answer to all answers list
                }
            }
        }

        return allAnswers;
    }

    public string GetDeckName()
    {
        return deckName;
    }

    public void SetDeckName(string name)
    {
        deckName = name;
    }
}
