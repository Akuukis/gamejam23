using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDamage : MonoBehaviour
{
    public float impactDamage = 10f;
    public GameObject impactParticlePrefab;

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
}
