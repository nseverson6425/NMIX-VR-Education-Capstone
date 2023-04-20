using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] GameObject shootPoint;
    [SerializeField] SlideGameController gameController;
    [SerializeField] GameObject helpText;
    [SerializeField] float launchVelocity = 10f;
    [SerializeField] bool startGame = false;

    private bool startedGame;

    private void Start()
    {
        startedGame = false;
    }

    private void Update()
    {
        if (startGame)
        {
            startGame = false;
            TriggerGameStart();
        }
    }

    public void LaunchProjectile()
    {
        // Instantiate the prefab at the given position
        GameObject newObject = Instantiate(projectilePrefab, shootPoint.transform.position, Quaternion.identity);
        Destroy(newObject, 2f);

        // Set the parent of the new object to be the same as the parent of this script's game object
        newObject.transform.parent = transform.parent;

        // Get the rigidbody component of the new object
        Rigidbody rb = newObject.GetComponent<Rigidbody>();

        // Check if the new object has a rigidbody component
        if (rb != null)
        {
            // Calculate the velocity vector based on the given speed and negative y direction
            Vector3 velocity = -transform.up * launchVelocity;

            // Launch the new object in the direction of the velocity vector
            rb.velocity = velocity;
        }
        else
        {
            // Log an error if the new object does not have a rigidbody component
            Debug.LogError("New object does not have a Rigidbody component");
        }
    }

    public void TriggerGameStart()
    {
        if (!startedGame)
        {
            helpText.SetActive(false);
            startedGame = true;
            gameController.StartGame();
        }
    }
}
