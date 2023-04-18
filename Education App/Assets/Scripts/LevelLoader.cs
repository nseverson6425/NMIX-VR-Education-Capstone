using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void LoadSlideGame()
    {
        SceneManager.LoadScene("Slide Game");
    }

    public void LoadBowGame() {
        SceneManager.LoadScene("BowAndArrow");
    }
}
