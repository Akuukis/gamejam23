using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageableObject : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth = 100f;  // Maximum health value that can be set up in the Inspector
	[SerializeField] private GameObject deathParticleEffect;  // Particle effect prefab assigned in the Inspector
	[SerializeField] private Slider healthSlider;  // Reference to the UI Slider component assigned in the Inspector
    private float currentHealth;
	public GameObject impactParticlePrefab;
	
	private void Start()
    {
        currentHealth = maxHealth;
		UpdateHealthBar();
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
}