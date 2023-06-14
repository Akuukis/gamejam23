using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDamage : MonoBehaviour
{
    public float impactDamage = 10f;
    public GameObject impactParticlePrefab;
	CameraShaker cameraShaker;

    private void Start()
	{
		cameraShaker = FindObjectOfType<CameraShaker>();
	}
	
	private void OnCollisionEnter(Collision collision)
    {
        // Apply damage to the object being collided with (if it implements IDamageable)
        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(impactDamage);
        }

		if (impactParticlePrefab == null)
    {
        Debug.LogWarning("No impact particle prefab assigned!");
        return;
    }

        // Instantiate the impact particle effect at the collision point
        GameObject impactParticle = Instantiate(impactParticlePrefab, transform.position, Quaternion.identity);
        Destroy(impactParticle, 2f); // Optionally, destroy the particle effect after 2 seconds
		Destroy(gameObject);
    }
	
	private void OnTriggerEnter(Collider other)
    {
        IDamageable damageable = other.gameObject.GetComponent<IDamageable>();

        if (damageable != null)
        {
            damageable.TakeDamage(impactDamage);
			bool isHighDamage = true; // Determine if it's a high damage situation or not

			// Shake the camera with the appropriate shake settings
			cameraShaker.ShakeCamera(isHighDamage);
        }
		
		if (impactParticlePrefab == null)
		{
			Debug.LogWarning("No impact particle prefab assigned!");
			return;
		}

			// Instantiate the impact particle effect at the collision point
			GameObject impactParticle = Instantiate(impactParticlePrefab, transform.position, Quaternion.identity);
			Destroy(impactParticle, 2f); // Optionally, destroy the particle effect after 2 seconds
			Destroy(gameObject);
	}
}