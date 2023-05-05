using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckController : MonoBehaviour
{
    [SerializeField] SlideGameController gameController;
    [SerializeField] GameObject buttonPrefab;
    [SerializeField] GameObject buttonParent;

    private List<Deck> decks;

    public bool triggerStartGame = false;

    // Start is called before the first frame update
    void Start()
    {
        // initialize variables
        decks = new List<Deck>();

        // create water cycle deck
        //Debug.Log("Created Water Cycle Deck");
        Deck waterCycleDeck = new Deck();
        decks.Add(waterCycleDeck); // add to stored decks
        waterCycleDeck.SetDeckName("Water Cycle"); // set deck name
        
        waterCycleDeck.AddSet("The water cycle is also called the ... ?", 
            waterCycleDeck.CreateAnswerList("Hydrologic Cycle"), 
            Set.QuestionType.MultipleChoice);
        waterCycleDeck.AddSet("... is the process that turns water vapor into a liquid which causes the formation of a cloud.", 
            waterCycleDeck.CreateAnswerList("Condensation"), 
            Set.QuestionType.MultipleChoice);
        waterCycleDeck.AddSet("After it rains, the water can either end up on land or ... ?", 
            waterCycleDeck.CreateAnswerList("In a body of water"), 
            Set.QuestionType.MultipleChoice);
        waterCycleDeck.AddSet("When water evaporates from a leaf, the process is called ... ?",
            waterCycleDeck.CreateAnswerList("Transpiration"), 
            Set.QuestionType.MultipleChoice);
        waterCycleDeck.AddSet("When water is heated in an ocean, the liquid water changes form and turns into ... ?",
            waterCycleDeck.CreateAnswerList("Water Vapor"), 
            Set.QuestionType.MultipleChoice);
        waterCycleDeck.AddSet("When water leaves a body of water after it is heated, the process is called ... ?",
            waterCycleDeck.CreateAnswerList("Evaporation"), 
            Set.QuestionType.MultipleChoice);
        waterCycleDeck.AddSet("When water falls from the sky, the process is known as ... ?",
            waterCycleDeck.CreateAnswerList("Precipitation"), 
            Set.QuestionType.MultipleChoice);
        waterCycleDeck.AddSet("When water hits land and is soaked into the ground, the water becomes ... ?",
            waterCycleDeck.CreateAnswerList("Ground Water"), 
            Set.QuestionType.MultipleChoice);
        waterCycleDeck.AddSet("True or False: The water cycle is a continual process?",
            waterCycleDeck.CreateAnswerList("True"), 
            Set.QuestionType.TrueOrFalse);
        waterCycleDeck.AddSet("True or False: Transpiration is a process that occurs on plants and animals?",
            waterCycleDeck.CreateAnswerList("False"), 
            Set.QuestionType.TrueOrFalse);


        // DEMO Deck CREATION
        Deck scienceDeck = new Deck();
        decks.Add(scienceDeck);
        scienceDeck.SetDeckName("Auburn Advanced Science");

        scienceDeck.AddSet("What noise does a cow make?",
            scienceDeck.CreateAnswerList("Moo"),
            scienceDeck.CreateAnswerList("Oink", "Cluck", "Woof"),
            Set.QuestionType.StrictAnswers);

        scienceDeck.AddSet("What do bees make?",
            scienceDeck.CreateAnswerList("Honey"),
            scienceDeck.CreateAnswerList("Ketchup", "Milk", "H2O"),
            Set.QuestionType.StrictAnswers);

        scienceDeck.AddSet("What does a thermometer measure?",
            scienceDeck.CreateAnswerList("Temperature"),
            scienceDeck.CreateAnswerList("Clouds", "Wind", "Cats"),
            Set.QuestionType.StrictAnswers);

        scienceDeck.AddSet("What is the largest continent?",
            scienceDeck.CreateAnswerList("Asia"),
            scienceDeck.CreateAnswerList("Europe", "Africa", "Florida"),
            Set.QuestionType.StrictAnswers);

        // create test deck
        Deck historyDeck = new Deck();
        decks.Add(historyDeck);
        historyDeck.SetDeckName("History");

        historyDeck.AddSet("What happened in 1066?",
            historyDeck.CreateAnswerList("The Battle of Hastings"),
            historyDeck.CreateAnswerList("WW2", "The discovery of America", "Absolutely nothing"),
            Set.QuestionType.StrictAnswers);

        historyDeck.AddSet("Which of these was NOT a Roman Leader?",
            historyDeck.CreateAnswerList("Caesar Salad"),
            historyDeck.CreateAnswerList("Julius Casear", "Augustus", "Nero"),
            Set.QuestionType.StrictAnswers);

        historyDeck.AddSet("Where was the Titanic headed to before it sank?",
            historyDeck.CreateAnswerList("The USA"),
            historyDeck.CreateAnswerList("Japan", "Mexico", "IDK"),
            Set.QuestionType.StrictAnswers);

        historyDeck.AddSet("Tomatoes, potatoes, chillies and many other things were brought to Europe in the 16th Century, they're not from Europe originally. Where did they come from?",
            historyDeck.CreateAnswerList("The Americas"),
            historyDeck.CreateAnswerList("Africa", "A giant underground cave", "Outer space"),
            Set.QuestionType.StrictAnswers);

        historyDeck.AddSet("Who was the third man to walk on the moon?",
            historyDeck.CreateAnswerList("Charles 'Pete' Conrad"),
            historyDeck.CreateAnswerList("Neil Armstrong", "Mark Hamil", "Astronaut Audrey"),
            Set.QuestionType.StrictAnswers);

        historyDeck.AddSet("Who did Henry VIII first marry?",
            historyDeck.CreateAnswerList("Catherine of Aragon"),
            historyDeck.CreateAnswerList("Meghan Markle", "Kate Middleton", "Holly Willoughby"),
            Set.QuestionType.StrictAnswers);

        // create button for decks
        CreateButtonOptions();
    }

    private void CreateButtonOptions()
    {
        foreach (Deck deck in decks)
        {
            // create button and add to scrollable list
            GameObject deckOption = Instantiate(buttonPrefab, buttonParent.transform);

            // if selected this button will start the game using 'this' deck
            deckOption.GetComponent<Button>().onClick.AddListener(() => StartGame(deck));

            // set button text
            deckOption.GetComponent<DeckButton>().deckTitle.text = deck.GetDeckName();

            //Debug.Log("Finished generating deck: " + deck.GetDeckName());
        }
    }

    private void StartGame(Deck selection)
    {
        gameController.PrepareGameStart(selection);
    }
}
