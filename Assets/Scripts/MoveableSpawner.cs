using System.Collections.Generic;
using UnityEngine;

public class MoveableSpawner : MonoBehaviour
{
    public bool prespawn = false;
    public float moveSpeed = 12f;
    public float spawnStep = 10f;
    public float spawnMinDist = 10f;
    public float spawnChance = 1f;
    public float spawnAt = 10f;
    public float despawnAt = -10f;
    public List<MoveableSpawnable> spawnables = new List<MoveableSpawnable>();
    protected float distanceLastStep = 0;
    protected float distanceLastSpawn = -9999;
    protected float totalWeight = 0;

    protected void Start()
    {
        foreach (MoveableSpawnable spawnable in spawnables)
        {
            totalWeight += spawnable.spawnWeight;
        }


        if(prespawn)
        {
            for(float i=-spawnAt; i<=0; i=i+spawnStep)
            {
                spawn(i);
            }
        }
    }

    protected void Update()
    {
        float newDistance = Time.time * moveSpeed;
        if(newDistance > distanceLastStep)
        {
            distanceLastStep = distanceLastStep + spawnStep;
            spawn(distanceLastStep);
        }
    }

    protected void spawn(float distance) {
        if(Random.value > spawnChance) return;
        if(distanceLastSpawn + spawnMinDist > distance) return;
        distanceLastSpawn = distance;

        GameObject newGameObject = Instantiate(getWeightedRandomSpawnable().gameObject, transform);
        Moveable moveable = newGameObject.AddComponent(typeof(Moveable)) as Moveable;
        moveable.distance = distance + spawnAt;
        moveable.moveSpeed = moveSpeed;
        moveable.despawnAt = despawnAt;
        moveable.Update();
        newGameObject.SetActive(true);
    }

    public MoveableSpawnable getWeightedRandomSpawnable()
    {
        float randomValue = Random.Range(0f, totalWeight);

        foreach (MoveableSpawnable spawnable in spawnables)
        {
            if (randomValue <= spawnable.spawnWeight) return spawnable;

            randomValue -= spawnable.spawnWeight;
        }

        // This point should never be reached unless the list is empty
        throw new System.Exception("Do not edit MoveableSpawnable spawnWeight during runtime.");
    }

}