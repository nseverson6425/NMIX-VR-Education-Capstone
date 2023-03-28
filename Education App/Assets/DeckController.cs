using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DeckController : MonoBehaviour
{
    public GameObject controlCanvas;
    public GameObject helpMenu;
    public GameObject deckSelector;
    public Button waterCycleButton;
    public SlideGameController gameController;

    public bool triggerStartGame = false;

    // Start is called before the first frame update
    void Start()
    {
        helpMenu.SetActive(false);
        controlCanvas.SetActive(false);
        waterCycleButton.onClick.AddListener(StartWaterCycleGame);
    }

    // Update is called once per frame
    void Update()
    {
     if (triggerStartGame)
        {
            triggerStartGame = false;
            StartWaterCycleGame();
        }   
    }

    private void StartWaterCycleGame()
    {
        deckSelector.SetActive(false);
        helpMenu.SetActive(true);
        gameController.StartGame("Water Cycle");
    }
}
