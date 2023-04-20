using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    [SerializeField] WallManager wallManager;
    public ObstacleStatus status = ObstacleStatus.Undefined;
    private SlideGameController controller;

    public enum ObstacleStatus
    {
        Passed,
        Failed,
        Undefined
    }

    // Start is called before the first frame update
    void Start()
    {
        wallManager.SetController(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetGameController(SlideGameController c)
    {
        controller = c;
    }

    public void PlayerHitByWall()
    {
        Debug.Log("Obstacle Controller: Player hit wall");
        wallManager.endGame = true; // stop the game
        wallManager.ResetWallPositions(); // delete all walls
        status = ObstacleStatus.Failed; // flag obstacle as failed
        
    }

    public void StartDodgeMinigame()
    {
        wallManager.StartWallSpawning(); // start game
    }

    // player dodged all the obstacles
    public void EndDodgeMinigame()
    {
        status = ObstacleStatus.Passed; // flag obstacle as passed
    }
}
