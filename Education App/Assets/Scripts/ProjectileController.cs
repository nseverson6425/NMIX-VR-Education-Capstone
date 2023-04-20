using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this object will be a bomb that explodes when it hits an answer
//otherwise it will disappear if it misses its target and then respawn a new bomb/projectile that the user can use

public class ProjectileController : MonoBehaviour
{
    public ParticleSystem explosionEffect; // reference to particle system with explosion effect
    public bool causeExplosion = false;

    private void Update()
    {
        if (causeExplosion)
        {
            causeExplosion = false;
            Explode();
        }
    }

    public void Explode()
    {
        Instantiate(explosionEffect, transform.position, Quaternion.identity);
        Destroy(gameObject); // destroy self
    }
}
