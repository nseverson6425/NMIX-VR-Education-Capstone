using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public LevelLoader levelLoader;
    public Button slideGameButton;
    public Button arrowGameButton;
    public Button optionsButton;
    public Button creditsButton;
    public Button quitGameButton;
    public float loadLevelDelayInSeconds = 1.5f;
    public bool promptLoadSlideGame = false;
    
    // Start is called before the first frame update
    void Start()
    {
        slideGameButton.onClick.AddListener(LoadSlideGame);
        arrowGameButton.onClick.AddListener(LoadArrowGame);
    }

    // Update is called once per frame
    void Update()
    {
        if (promptLoadSlideGame)
        {
            promptLoadSlideGame = false;
            LoadSlideGame();
        }
    }

    private void LoadSlideGame()
    {
        StartCoroutine(LoadSlideGameDelay(loadLevelDelayInSeconds));
    }

    private IEnumerator LoadSlideGameDelay(float delayInSeconds)
    {
        yield return new WaitForSeconds(delayInSeconds);
        levelLoader.LoadSlideGame();
    }

    private void LoadArrowGame() 
    {
        levelLoader.LoadArrowGame();
    }
}
