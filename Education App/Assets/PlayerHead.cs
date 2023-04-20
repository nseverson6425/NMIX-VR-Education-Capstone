using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHead : MonoBehaviour
{
    [SerializeField] ObstacleController controller;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Wall")
        {
            controller.PlayerHitByWall(); // failed obstacle
        }
    }
}
