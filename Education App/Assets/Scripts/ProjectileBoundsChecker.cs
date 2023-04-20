using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBoundsChecker : MonoBehaviour
{
    public SlideGameController gameController; // reference to game controller
    private void OnTriggerEnter(Collider other)
    {
        ProjectileController pc = other.GetComponent<ProjectileController>();
        if (pc != null)
        {
            Destroy(pc.gameObject); // destroy the falling projectile
        }
    }
}
