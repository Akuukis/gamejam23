using System.Collections;
using System.Collections.Generic;
using UnityEngine;

	
public class DestroyObjectOnCollision : MonoBehaviour
{
	public GameObject impactParticlePrefab;
	
    private void OnTriggerEnter(Collider other)
    {

		// Instantiate the impact particle effect at the collision point
		GameObject impactParticle = Instantiate(impactParticlePrefab, transform.position, Quaternion.identity);
		Destroy(impactParticle, 2f); // Optionally, destroy the particle effect after 2 seconds
		Destroy(gameObject);
	}
}
