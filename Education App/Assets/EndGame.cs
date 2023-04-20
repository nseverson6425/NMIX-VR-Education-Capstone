using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    [SerializeField] SlideGameController gameManager;

    private void OnTriggerEnter(Collider other)
    {
        ProjectileController pc = other.GetComponent<ProjectileController>();
        if (pc != null)
        {
            gameManager.QuitGame();
        }
    }
}
