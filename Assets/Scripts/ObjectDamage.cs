using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDamage : MonoBehaviour
{
    public float impactDamage = 10f;

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collision is with a GameObject that can take damage
        IDamageable damageable = collision.collider.GetComponent<IDamageable>();
        if (damageable != null)
        {
            // Apply damage to the collided object
            damageable.TakeDamage(impactDamage);
        }

        // Destroy the thrown object upon impact
        Destroy(gameObject);
    }
}
