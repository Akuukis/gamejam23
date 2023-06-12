using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;

    private float vDirection;
    private float hDirection;

    private bool isMoving = false;

    public Transform turret;

    void Start()
    {
        turret = gameObject.transform.Find("Turret");
    }

    void Update()
    {
        if(Input.GetAxis("Vertical") != 0 && isMoving == false)
        {
            isMoving = true;
            vDirection = Input.GetAxisRaw("Vertical");
            StartCoroutine(GoVerticaly());
        }

        if(Input.GetAxis("Horizontal") != 0 && isMoving == false)
        {
            isMoving = true;
            hDirection = Input.GetAxisRaw("Horizontal");
            StartCoroutine(GoHorizontaly());
        }

        Vector3 positionOnScreen = Camera.main.WorldToViewportPoint(turret.position);
        Vector3 mouseOnScreen = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        float angle = AngleBetweenTwoPoints(positionOnScreen, mouseOnScreen);
        turret.rotation =  Quaternion.Euler (new Vector3(0f, -angle, 0f));

        Debug.DrawLine(positionOnScreen, mouseOnScreen);
    }
 
    float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }

    IEnumerator GoVerticaly()
    {
        Vector3 newPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z + 3.5f * vDirection);

        if(newPosition.z > 3.5f)
            newPosition.z = 3.5f;

        if(newPosition.z < -3.5f)
            newPosition.z = -3.5f;

        while(transform.position != newPosition)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, newPosition, step);
            yield return null;
        }

        isMoving = false;
        
        yield return null;
    }

    IEnumerator GoHorizontaly()
    {
        Vector3 newPosition = new Vector3(transform.position.x + 3.5f * hDirection, transform.position.y, transform.position.z);

        if(newPosition.x > 3.5f)
            newPosition.x = 3.5f;

        if(newPosition.x < -3.5f)
            newPosition.x = -3.5f;

        while(transform.position != newPosition)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, newPosition, step);
            yield return null;
        }

        isMoving = false;
        
        yield return null;
    }
}
