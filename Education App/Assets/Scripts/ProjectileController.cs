using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

//this object will be a bomb that explodes when it hits an answer
//otherwise it will disappear if it misses its target and then respawn a new bomb/projectile that the user can use

public class ProjectileController : MonoBehaviour
{
    public ParticleSystem explosionEffect; // reference to particle system with explosion effect
    public ParticleSystem smokeEffect; // reference to continuous smoke effect
    public bool causeExplosion = false;
    public AudioSource explosionSound; 

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
        //explosionSound.Play();
        smokeEffect.Stop(); // stop smoke effect
        explosionEffect.Play(); // play effect
        GetComponent<MeshRenderer>().forceRenderingOff = true;
        Destroy(gameObject, 3f); // destroy self
    }
}
