using UnityEngine;

public class MovingTiles : MonoBehaviour
{
    public float scrollSpeed = 1f;
    private float z;
    private Transform tile1;
    private Transform tile2;
    private float tileSize;

    private void Start()
    {
        tile1 = transform.GetChild(0);
        tile2 = Instantiate(tile1, tile1.position, tile1.rotation, transform);
        tileSize = tile1.GetComponent<Renderer>().bounds.size.z;
        z = tile1.position.z - tileSize;
    }

    private void Update()
    {
        float offset = Time.time * scrollSpeed;
        float newPosition1 = Mathf.Repeat(offset, tileSize * 2);
        float newPosition2 = Mathf.Repeat(offset + tileSize, tileSize * 2);
        tile2.localPosition = new Vector3(0, 0, z + tileSize * 2 - newPosition2);
        tile1.localPosition = new Vector3(0, 0, z + tileSize * 2 - newPosition1);
    }
}