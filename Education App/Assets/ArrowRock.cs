using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ArrowRock : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        ProjectileController pc = other.GetComponent<ProjectileController>();
        if (pc != null)
        {
            SceneManager.LoadScene("BowAndArrow");
        }
    }
}
