using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectThrower : MonoBehaviour
{
    public GameObject objectToThrow;
    public float throwAngle = 45f;
    public float throwForce = 10f;
    public float impactDamage = 10f;

    public void ThrowObject()
    {
		Debug.Log("This is the first log.");
		
        // Instantiate the object to throw
        GameObject thrownObject = Instantiate(objectToThrow, transform.position, Quaternion.identity);

        // Calculate the initial velocity for the arc trajectory
        float throwVelocity = throwForce / Mathf.Sin(2f * throwAngle * Mathf.Deg2Rad);
        Vector3 throwDirection = transform.forward + (transform.up * Mathf.Sin(throwAngle * Mathf.Deg2Rad));
        Vector3 initialVelocity = throwDirection.normalized * throwVelocity;

        // Apply the calculated velocity to the thrown object's Rigidbody component
        Rigidbody rb = thrownObject.GetComponent<Rigidbody>();
        rb.velocity = initialVelocity;

        // Add a script to the thrown object to apply damage upon impact
        ObjectDamage objectDamage = thrownObject.AddComponent<ObjectDamage>();
        objectDamage.impactDamage = impactDamage;
		
		Debug.Log("This is after what ever.");
    }
}
