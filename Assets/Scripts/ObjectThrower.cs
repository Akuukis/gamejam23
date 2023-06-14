using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectThrower : MonoBehaviour
{
    public List<GameObject> objectsToThrow;

    public float throwAngle = 45f;
    public float throwForce = 10f;
    public float impactDamage = 10f;
    public float maxRotationSpeed = 5f;

    public GameObject GetRandomObjectToThrow()
    {
        if (objectsToThrow.Count == 0)
        {
            Debug.LogWarning("No objects to throw!");
            return null;
        }

        int randomIndex = Random.Range(0, objectsToThrow.Count);
        return objectsToThrow[randomIndex];
    }

    public void ThrowObject()
    {
        GameObject objectToThrow = GetRandomObjectToThrow();
        if (objectToThrow == null)
        {
            return;
        }

        // Instantiate the selected object to throw
        GameObject thrownObject = Instantiate(objectToThrow, transform.position, Quaternion.identity);

        // Apply a random rotation spin to the thrown object
        Rigidbody rb = thrownObject.GetComponent<Rigidbody>();
        Vector3 randomRotation = Random.insideUnitSphere * maxRotationSpeed;
        rb.angularVelocity = randomRotation;

        // Calculate the initial velocity for the arc trajectory
        float throwVelocity = throwForce / Mathf.Sin(2f * throwAngle * Mathf.Deg2Rad);
        Vector3 throwDirection = transform.forward + (transform.up * Mathf.Sin(throwAngle * Mathf.Deg2Rad));
        Vector3 initialVelocity = throwDirection.normalized * throwVelocity;

        // Apply the calculated velocity to the thrown object's Rigidbody component
        rb.velocity = initialVelocity;

        // Add a script to the thrown object to apply damage upon impact
        ObjectDamage objectDamage = thrownObject.AddComponent<ObjectDamage>();
        objectDamage.impactDamage = impactDamage;
    }
}