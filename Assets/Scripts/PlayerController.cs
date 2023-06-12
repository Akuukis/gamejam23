using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed = 5f;

    [Header("Grid Size")]
    public int xValue = 1;
    public int zValue = 1;

    private float vDirection;
    private float hDirection;

    public bool isMoving = false;

    private Transform turret;
    public Collider col;
    public Collider trigger;

    public bool canMove = true;

    void Start()
    {
        turret = gameObject.transform.Find("Turret");
    }

    void Update()
    {
        if(canMove)
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

        if(isMoving)
            trigger.enabled = true;
        else
            trigger.enabled = false;

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 11f;
        Vector3 worldMouse = Camera.main.ScreenToWorldPoint(mousePos);
        
        Vector3 turretOrientation = worldMouse - turret.position;
        turretOrientation.y = 0f;
        turret.forward = turretOrientation;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && other.GetComponent<PlayerController>().isMoving == false)
        {
            other.GetComponent<PlayerImpact>().GotHit();
            col.enabled = false;
        }
    }

    IEnumerator GoVerticaly()
    {
        Vector3 newPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z + 3.5f * vDirection);

        if(newPosition.z > 3.5f * zValue)
            newPosition.z = 3.5f * zValue;

        if(newPosition.z < -3.5f * zValue)
            newPosition.z = -3.5f * zValue;

        while(transform.position != newPosition)
        {
            float step = movementSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, newPosition, step);
            yield return null;
        }

        isMoving = false;
        yield return null;
    }

    IEnumerator GoHorizontaly()
    {
        Vector3 newPosition = new Vector3(transform.position.x + 3.5f * hDirection, transform.position.y, transform.position.z);

        if(newPosition.x > 3.5f * xValue)
            newPosition.x = 3.5f * xValue;

        if(newPosition.x < -3.5f * xValue)
            newPosition.x = -3.5f * xValue;

        while(transform.position != newPosition)
        {
            float step = movementSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, newPosition, step);
            yield return null;
        }

        isMoving = false;
        yield return null;
    }
}
