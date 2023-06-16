using System.Collections.Generic;
using UnityEngine;

public class MoveableSpawner : MonoBehaviour
{
    public int prespawnCount = 3;
    public float prespawnOffset = 10f;
    public float moveSpeed = 12f;
    public float spawnAtFrom = 20f;
    public float spawnAtTo = 20f;
    public float despawnAt = -10f;
    public List<MoveableSpawnable> spawnables = new List<MoveableSpawnable>();
    protected float totalWeight = 0;

    protected void Start()
    {
        foreach (MoveableSpawnable spawnable in spawnables)
        {
            totalWeight += spawnable.spawnWeight;
        }

        for(float i=0; i<prespawnCount; i++)
        {
            spawn(-i * prespawnOffset / 2);
        }
    }

    public void spawn(float offset = 0f) {
        GameObject newGameObject = Instantiate(getWeightedRandomSpawnable().gameObject, transform);
        Moveable moveable = newGameObject.AddComponent(typeof(Moveable)) as Moveable;
        float newDistance = Time.time * moveSpeed;
        moveable.distance = newDistance + Random.Range(spawnAtFrom, spawnAtTo) + offset;
        moveable.moveSpeed = moveSpeed;
        moveable.despawnAt = despawnAt;
        moveable.spawner = transform.gameObject.GetComponent<MoveableSpawner>();
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