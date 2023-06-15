using System.Collections.Generic;
using UnityEngine;

public class MoveableSpawner : MonoBehaviour
{
    public float moveSpeed = 12f;
    public float spawnEvery = 10f;
    public float spawnChance = 1f;
    public float spawnAt = 10f;
    public float despawnAt = -10f;
    protected List<MoveableSpawnable> spawnables = new List<MoveableSpawnable>();
    protected float distance = 0;
    protected float totalWeight = 0;

    protected void Start()
    {
        foreach (Transform child in transform)
        {
            MoveableSpawnable spawnable = child.GetComponent<MoveableSpawnable>();
            if(spawnable == null) continue;

            spawnables.Add(spawnable);
            totalWeight += spawnable.spawnWeight;
            spawnable.gameObject.SetActive(false);
        }

        //// Leave hardcoded value, because the formula below doesn't work on Collider.
        // tileSize = tiles[tile1].GetComponent<Renderer>().localBounds.size.z;

        for(float i=0; i<=spawnAt; i=i+spawnEvery)
        {
            spawn(i);
        }
    }

    protected void Update()
    {
        float newDistance = Time.time * moveSpeed;
        if(newDistance > distance)
        {
            distance = distance + spawnEvery;
            spawn(distance);
        }
    }

    protected void spawn(float distance) {
        if(Random.value > spawnChance) return;

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