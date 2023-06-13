using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageableObject : MonoBehaviour, IDamageable
{
    public void TakeDamage(float amount)
    {
        // Implement the damage application logic for the object
        // You can modify this method based on your specific requirements
        Debug.Log($"{gameObject.name} took {amount} damage.");
    }
}
