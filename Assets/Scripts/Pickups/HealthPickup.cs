using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public float healthAmount = 50f;
	public GameObject impactParticlePrefab;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DamageableObject damageableObject = other.GetComponent<DamageableObject>();
            if (damageableObject != null)
            {
                damageableObject.Heal(healthAmount);

                Destroy(gameObject);
            }
        }
		
		// Instantiate the impact particle effect at the collision point
		GameObject impactParticle = Instantiate(impactParticlePrefab, transform.position, Quaternion.identity);
		Destroy(impactParticle, 2f); // Optionally, destroy the particle effect after 2 seconds
		Destroy(gameObject);
    }
}
