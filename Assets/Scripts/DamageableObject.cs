using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageableObject : MonoBehaviour, IDamageable
{
    public GameObject impactParticlePrefab;
	
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
    }
}
