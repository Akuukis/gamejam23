using System.Collections.Generic;
using UnityEngine;

public class MovingTiles : MonoBehaviour
{
    public float scrollSpeed = 10f;
    private float z;
    private int tile1 = 0;
    private int tile2 = 1;
    private List<Transform> tiles = new List<Transform>();
    private float tileSize;
    private float distance = 0;

    private void Start()
    {
        foreach (Transform child in transform)
        {
            tiles.Add(child);
            child.gameObject.SetActive(false);
        }

        tileSize = tiles[tile1].GetComponent<Renderer>().localBounds.size.z;
        z = tiles[tile1].position.z - tileSize;
    }

    private void Update()
    {
        float offset = Time.time * scrollSpeed;
        if(offset > distance)
        {
            distance = distance + tileSize;
            if(Mathf.Repeat(distance, tileSize * 2) == 0)
            {
                tiles[tile2].gameObject.SetActive(false);
                tile2 = (int)(Random.value * (transform.childCount - 1));
                if(tile2 == tile1) tile2 = (tile2 + 1) % transform.childCount; // Always use different tiles.
                tiles[tile2].gameObject.SetActive(true);
            } else {
                tiles[tile1].gameObject.SetActive(false);
                tile1 = (int)(Random.value * (transform.childCount - 1));
                if(tile1 == tile2) tile1 = (tile1 + 1) % transform.childCount; // Always use different tiles.
                tiles[tile1].gameObject.SetActive(true);
            }
        }
        float newPosition1 = Mathf.Repeat(offset, tileSize * 2);
        float newPosition2 = Mathf.Repeat(offset + tileSize, tileSize * 2);
        tiles[tile1].localPosition = new Vector3(0, 0, z + tileSize * 2 - newPosition1);
        tiles[tile2].localPosition = new Vector3(0, 0, z + tileSize * 2 - newPosition2);
    }
}