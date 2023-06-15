using UnityEngine;

public class Moveable : MonoBehaviour
{
    public float moveSpeed = 12f;
    public float distance;
    public float despawnAt = -10f;
    protected void Update()
    {
        float newDistance = Time.time * moveSpeed;
        float newRelativeZ = distance - newDistance;
        if(newRelativeZ > despawnAt)
        {
            transform.localPosition = new Vector3(0, 0, newRelativeZ);
        } else {
            Destroy(gameObject);
        }
    }
}