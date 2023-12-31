using UnityEngine;

public class Moveable : MonoBehaviour
{
    public float moveSpeed = 12f;
    public float distance;
    public float despawnAt = -10f;

    public MoveableSpawner spawner;
    public void Update()
    {
        float newDistance = Time.time * moveSpeed;
        float newRelativeZ = distance - newDistance;
        if(newRelativeZ > despawnAt)
        {
            transform.localPosition = new Vector3(0, 0, newRelativeZ);
        } else {
            spawner.spawn();
            Destroy(gameObject);
        }
    }

}