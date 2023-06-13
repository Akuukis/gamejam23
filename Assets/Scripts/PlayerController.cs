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

    public bool isAccelerating = false;
    public bool isMoving = false;
    public bool isGrinding = false;

    private Transform turret;
    public Collider col;
    public Collider trigger;

    public bool canBeDamaged = true;

    [HideInInspector]
    public Vector3 oldPosition;
    private Vector3 newPosition;

    private IEnumerator activeCorountine;

    void Start()
    {
        turret = gameObject.transform.Find("Turret");
    }

    void Update()
    {
        if(Input.GetAxis("Vertical") != 0)
        {
            isAccelerating = true;
            if(isMoving == false)
            {
                isMoving = true;
                vDirection = Input.GetAxisRaw("Vertical");

                activeCorountine = GoVerticaly();
                StartCoroutine(activeCorountine);
            }
        }

        if(Input.GetAxis("Horizontal") != 0)
        {
            isAccelerating = true;
            if(Input.GetAxis("Horizontal") != 0 && isMoving == false)
            {
                isMoving = true;
                hDirection = Input.GetAxisRaw("Horizontal");

                activeCorountine = GoHorizontaly();
                StartCoroutine(activeCorountine);
            }
        }

        if(Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") == 0)
        {
            isAccelerating = false;
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

        if(isGrinding)
        {
            if(Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") == 0)
            {
                isGrinding = false;
                activeCorountine = MoveBackToOldPosition();
                StartCoroutine(activeCorountine);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && other.GetComponent<TestScript>().isMoving == false && canBeDamaged == true)
        {
            canBeDamaged = false;
            trigger.enabled = false;
            Debug.Log(other + " was hit away!");
            other.GetComponent<PlayerImpact>().GotHit();
            // col.enabled = false;
        }
        else if(other.tag == "Player" && other.GetComponent<TestScript>().isMoving == true && canBeDamaged == true)
        {
            canBeDamaged = false;
            trigger.enabled = false;
            if(isAccelerating == false && other.GetComponent<TestScript>().isAccelerating == false)
            {
                Debug.Log(this + " had a bounce!");
                StopCoroutine(activeCorountine);
                activeCorountine = MoveBackToOldPosition();
                StartCoroutine(activeCorountine);
            }
            else if(isAccelerating == true && other.GetComponent<TestScript>().isAccelerating == true)
            {
                Debug.Log(this + " is fighting back!");
                StopCoroutine(activeCorountine);
                isGrinding = true;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {

        if(isGrinding == false && other.GetComponent<TestScript>().isGrinding == true)
        {
            Debug.Log(this + " has lost!");
            StopCoroutine(activeCorountine);
            gameObject.GetComponent<PlayerImpact>().LooseThePosition();
        }
        else if(isGrinding == true && other.GetComponent<TestScript>().isGrinding == false)
        {
            Debug.Log(this + " has won!");
            StopCoroutine(activeCorountine);
            activeCorountine = WinThePosition();
            StartCoroutine(activeCorountine);
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
        canBeDamaged = true;
        yield return null;
    }

    IEnumerator GoHorizontaly()
    {
        oldPosition = transform.position;
        newPosition = new Vector3(transform.position.x + 3.5f * hDirection, transform.position.y, transform.position.z);

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
        canBeDamaged = true;
        yield return null;
    }

    IEnumerator MoveBackToOldPosition()
    {
        Debug.Log(oldPosition);
        while(transform.position != oldPosition)
        {
            float step = movementSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, oldPosition, step);
            yield return null;
        }

        isMoving = false;
        canBeDamaged = true;
        yield return null;
    }

    IEnumerator WinThePosition()
    {
        Debug.Log(newPosition);
        while(transform.position != newPosition)
        {
            float step = movementSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, newPosition, step);
            yield return null;
        }

        isMoving = false;
        canBeDamaged = true;
        yield return null;
    }
}
