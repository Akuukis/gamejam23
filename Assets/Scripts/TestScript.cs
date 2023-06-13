using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public float movementSpeed = 5f;

    [Header("Grid Size")]
    public int xValue = 1;
    public int zValue = 1;

    private float vDirection;
    private float hDirection;

    public bool isMoving = false;
    public bool isGrinding = false;

    private Transform turret;
    public Collider col;
    public Collider trigger;

    public bool canMove = true;

    private Vector3 oldPosition;
    private Vector3 newPosition;

    private IEnumerator activeCorountine;

    void Start()
    {
        turret = gameObject.transform.Find("Turret");
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.O) && isMoving == false)
        {
            isMoving = true;

            activeCorountine = GoHorizontaly(-1f);
            StartCoroutine(activeCorountine);
        }

        if(Input.GetKey(KeyCode.P) && isMoving == false)
        {
            isMoving = true;

            activeCorountine = GoHorizontaly(1f);
            StartCoroutine(activeCorountine);
        }

        if(canMove)
        {
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

        if(isGrinding)
        {
            if(Input.GetKey(KeyCode.O) == false && Input.GetKey(KeyCode.P) == false)
            {
                isGrinding = false;
                StartCoroutine(MoveBackToOldPosition());
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && other.GetComponent<Controller>().isMoving == false)
        {
            other.GetComponent<PlayerImpact>().GotHit();
            col.enabled = false;
        }

        if(other.tag == "Player" && other.GetComponent<Controller>().isMoving == true)
        {
            StopCoroutine(activeCorountine);
            isGrinding = true;
        }
    }

    void OnTriggerExit(Collider other)
    {

        if(isGrinding == false && other.GetComponent<Controller>().isGrinding == true)
        {
            Debug.Log(this + " Lost the position!");

            gameObject.GetComponent<PlayerImpact>().LooseThePosition();
        }
        else if(isGrinding == true && other.GetComponent<Controller>().isGrinding == false)
        {
            Debug.Log(this + " Won the position!");

            activeCorountine = WinThePosition();
            StartCoroutine(activeCorountine);
            //go to new position
            isGrinding = false;
        }
    }

    IEnumerator GoVerticaly()
    {
        oldPosition = transform.position;
        newPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z + 3.5f * vDirection);

        if(newPosition.z > 3.5f * zValue)
            newPosition.z = 3.5f * zValue;

        if(newPosition.z < -3.5f * zValue)
            newPosition.z = -3.5f * zValue;

        while(transform.position != newPosition && isGrinding == false)
        {
            float step = movementSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, newPosition, step);
            yield return null;
        }

        isMoving = false;
        yield return null;
    }

    IEnumerator GoHorizontaly(float xd)
    {
        oldPosition = transform.position;
        newPosition = new Vector3(transform.position.x + 3.5f * xd, transform.position.y, transform.position.z);

        if(newPosition.x > 3.5f * xValue)
            newPosition.x = 3.5f * xValue;

        if(newPosition.x < -3.5f * xValue)
            newPosition.x = -3.5f * xValue;

        while(transform.position != newPosition && isGrinding == false)
        {
            float step = movementSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, newPosition, step);
            yield return null;
        }

        isMoving = false;
        yield return null;
    }

    IEnumerator MoveBackToOldPosition()
    {
        while(transform.position != oldPosition)
        {
            float step = movementSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, oldPosition, step);
            yield return null;
        }

        isMoving = false;
        yield return null;
    }

    IEnumerator WinThePosition()
    {
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
