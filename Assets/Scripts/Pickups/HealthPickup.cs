using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public float healthAmount = 50f;

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
    }
}
