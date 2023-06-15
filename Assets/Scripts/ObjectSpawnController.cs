using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawnController : MonoBehaviour
{
    public ObjectThrower objectThrower;
    public Animator animator;

    private bool hasSpawned = false;

    private void Update()
    {
        // Check if the parameter "ReadyToThrow" is active
        bool readyToThrow = animator.GetBool("ReadyToThrow");

        // If the parameter is active and the object hasn't been spawned yet, spawn it
        if (readyToThrow && !hasSpawned)
        {
            SpawnObjectToThrow();
            hasSpawned = true;
        }

        // If the parameter is not active, reset the spawn flag
        if (!readyToThrow)
        {
            hasSpawned = false;
        }
    }

    private void ReadyToThrow()
    {
        objectThrower.isReady = true;
    }

    private void SpawnObjectToThrow()
    {
        objectThrower.CheckObjectToTrhowList();
        
        // Get a random object to throw from the ObjectThrower's list
        // GameObject objectToThrow = objectThrower.GetRandomObjectToThrow();

        // if (objectToThrow != null)
        // {
        //     // Instantiate the object to throw at the desired position and rotation
        //     Instantiate(objectToThrow, transform.position, transform.rotation);
        // }
        // else
        // {
        //     Debug.LogWarning("No objects to throw!");
        // }
    }
}
