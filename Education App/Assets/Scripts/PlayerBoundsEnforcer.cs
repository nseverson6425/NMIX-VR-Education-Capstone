using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBoundsEnforcer : MonoBehaviour
{
    public float respawnXPos = 0f;
    public float respawnYPos = 2f;
    public float respawnZPos = 0f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") // check if player object
        {
            other.transform.position = new Vector3(respawnXPos, respawnYPos, respawnZPos); // teleport to middle of platform
        }
    }
}
