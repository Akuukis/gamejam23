using System.Collections.Generic;
using UnityEngine;

public class MoveableSpawner : MonoBehaviour
{
    public float moveSpeed = 12f;
    public float spawnEvery = 10f;
    public float spawnAt = 10f;
    public float despawnAt = -10f;
    protected float z;
    protected List<GameObject> options = new List<GameObject>();
    protected List<Moveable> instances = new List<Moveable>();
    protected float distance = 0;

    protected void Start()
    {
        foreach (Transform child in transform)
        {
            options.Add(child.gameObject);
            child.gameObject.SetActive(false);
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
        int index = (int)(Random.value * options.Count);
        GameObject newGameObject = Instantiate(options[index], transform);
        newGameObject.SetActive(true);
        Moveable moveable = newGameObject.AddComponent(typeof(Moveable)) as Moveable;
        moveable.distance = distance + spawnAt;
        moveable.moveSpeed = moveSpeed;
        moveable.despawnAt = despawnAt;
        instances.Add(moveable);
    }

}