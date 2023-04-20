using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallManager : MonoBehaviour
{
    public Transform[] wallPositions;
    public GameObject wallPrefab;
    public float wallSpeed = 5f;
    public float wallSpawnDelay = 1f;
    public float gameDuration = 30f;
    public bool endGame = false;

    private float elapsedTime = 0f;
    private ObstacleController controller;

    private IEnumerator SpawnWalls()
    {
        endGame = false;

        while (elapsedTime <= gameDuration)
        {
            if (endGame)
            {
                yield break; // stop the game
            }

            elapsedTime += Time.deltaTime;
            int randomPosition = Random.Range(0, wallPositions.Length);
 
            Debug.Log("spawned wall");
            GameObject wall = Instantiate(wallPrefab, wallPositions[randomPosition].position, Quaternion.Euler(0f, 90f, 0f));
            wall.transform.parent = wallPositions[randomPosition];

            yield return new WaitForSeconds(wallSpawnDelay);
        }

        controller.EndDodgeMinigame();
        ResetWallPositions();
    }

    public void SetController(ObstacleController oc)
    {
        controller = oc;
    }


    void Update()
    {
        foreach (Transform wallPosition in wallPositions)
        {
            foreach (Transform wall in wallPosition)
            {
                wall.position += Vector3.right * wallSpeed * Time.deltaTime;
            }
        }
    }

    // starts spawning walls
    public void StartWallSpawning()
    {
        StartCoroutine(SpawnWalls());
    }

    public void ResetWallPositions()
    {
        Debug.Log("reset wall position");
        foreach (Transform wallPosition in wallPositions)
        {
            foreach (Transform wall in wallPosition)
            {
                Destroy(wall.gameObject);
            }
        }
    }
}

