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

        // create test deck
        Deck testDeck = new Deck();
        decks.Add(testDeck);
        testDeck.SetDeckName("Test Deck");


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
