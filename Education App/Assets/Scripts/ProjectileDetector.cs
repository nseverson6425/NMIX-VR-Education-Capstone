using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDetector : MonoBehaviour
{
    [SerializeField] AnswerChoice controller;

    private void OnTriggerEnter(Collider other)
    {
        ProjectileController projectile = other.GetComponent<ProjectileController>();
        if (projectile != null) // valid choice selection
        {
            projectile.Explode();
            controller.AlertChoice();
        }
    }
}
