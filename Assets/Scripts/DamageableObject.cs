using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageableObject : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth = 100f;  // Maximum health value that can be set up in the Inspector
	[SerializeField] private GameObject deathParticleEffect;  // Particle effect prefab assigned in the Inspector
	[SerializeField] private Slider healthSlider;  // Reference to the UI Slider component assigned in the Inspector
	[SerializeField] private ParticleSystem[] particleEffectSlots;  // Array of particle effect slots assigned in the Inspector
    private float currentHealth;
	private float previousHealthPercentage = 1f;  // Variable to store the previous health percentage
	public GameObject impactParticlePrefab;
	CameraShaker cameraShaker;
	
	private void Start()
    {
        currentHealth = maxHealth;
		UpdateHealthBar();
		cameraShaker = FindObjectOfType<CameraShaker>();
    }
	
	public void TakeDamage(float amount)
    {
        // Implement the damage application logic for the object
        // You can modify this method based on your specific requirements
        Debug.Log($"{gameObject.name} took {amount} damage.");
		
		// Play the impact particle effect on the damaged object
        if (impactParticlePrefab != null)
        {
            GameObject impactParticle = Instantiate(impactParticlePrefab, transform.position, Quaternion.identity);
            Destroy(impactParticle, 2f); // Optionally, destroy the particle effect after 2 seconds
        }
		
		currentHealth -= amount;

        if (currentHealth <= 0f)
        {
            Die();
        }
		
		UpdateHealthBar();
		UpdateParticleEffects();
    }
	
	private void Die()
    {
        // Play particle effect at the position of the object
        if (deathParticleEffect != null)
        {
            Instantiate(deathParticleEffect, transform.position, Quaternion.identity);
        }

        // Handle the object's death
        Destroy(gameObject);
		Destroy(healthSlider);
    }
	
	private void UpdateHealthBar()
    {
        if (healthSlider != null)
        {
            float normalizedHealth = currentHealth / maxHealth;
            healthSlider.value = normalizedHealth;
        }
    }
	
	private void UpdateParticleEffects()
	{
		if (particleEffectSlots.Length > 0)
		{
			float healthPercentage = currentHealth / maxHealth;

			for (int i = 0; i < particleEffectSlots.Length; i++)
			{
				ParticleSystem particleSystem = particleEffectSlots[i];

				if (healthPercentage <= (float)(i + 1) / particleEffectSlots.Length)
				{
					// Activate particle effect slot and all its children
					SetParticleSystemsPlayingState(particleSystem, true);
					
					bool isHighDamage = false; // Determine if it's a high damage situation or not

					// Shake the camera with the appropriate shake settings
					cameraShaker.ShakeCamera(isHighDamage);
					
				}
				else if (previousHealthPercentage > (float)(i + 1) / particleEffectSlots.Length)
				{
					// Deactivate particle effect slot and all its children
					SetParticleSystemsPlayingState(particleSystem, false);
				}
			}

			previousHealthPercentage = healthPercentage;
		}
	}

	private void SetParticleSystemsPlayingState(ParticleSystem particleSystem, bool play)
	{
		ParticleSystem[] childParticleSystems = particleSystem.GetComponentsInChildren<ParticleSystem>(true);

		foreach (ParticleSystem childParticleSystem in childParticleSystems)
		{
			if (play)
			{
				childParticleSystem.Play(true);
			}
			else
			{
				childParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
			}
		}
	}

}