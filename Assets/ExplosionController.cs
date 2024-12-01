using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    public ParticleSystem[] explosionEffects; // Reference to the ParticleSystem
    public float explosionSize = 1f;      // Size of the explosion in Unity units

    [SerializeField]
    AudioSource explosionAudioSource;

    public void TriggerExplosion(float size)
    {
        explosionSize = size; // Set the explosion size dynamically

        // Adjust the size of the particle effect
        foreach (ParticleSystem explosionEffect in explosionEffects)
        {
            var mainModule = explosionEffect.main;
            mainModule.startSizeMultiplier = size * 0.5f; // Scale particle size (adjust as needed)

            // Adjust the speed of particles
            mainModule.startSpeedMultiplier = size * 2f; // Scale particle velocity (adjust as needed)

            var shapeModule = explosionEffect.shape;
            shapeModule.radius = size;

            // Play the particle effect
            explosionEffect.Play();
        }
    }
}
