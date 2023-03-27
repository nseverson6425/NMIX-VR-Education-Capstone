using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this object will be a bomb that explodes when it hits an answer
//otherwise it will disappear if it misses its target and then respawn a new bomb/projectile that the user can use

public class ProjectileController : MonoBehaviour
{
    public ParticleSystem explosionEffect; // reference to particle system with explosion effect
    
    public void Explode()
    {
        explosionEffect.Play(); // play effect\
        GetComponent<MeshRenderer>().forceRenderingOff = true;
        Destroy(gameObject, 3f); // destroy self
    }
}
