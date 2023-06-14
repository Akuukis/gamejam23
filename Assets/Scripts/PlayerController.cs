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
    public bool canMove = true;
    
    public Transform turret;
    public Collider col;
    public Collider trigger;

    [HideInInspector]
    public Vector3 oldPosition;
    private Vector3 newPosition;

    public IEnumerator activeCorountine;
    private GameObject playerInContact;

    void Update()
    {
        if(Input.GetAxis("Vertical") != 0)
        {
            isAccelerating = true;
            if(isMoving == false && canMove == true)
            {
                trigger.enabled = true;
                isMoving = true;
                canMove = false;
                vDirection = Input.GetAxisRaw("Vertical");

                oldPosition = transform.position;
                activeCorountine = GoVerticaly();
                StartCoroutine(activeCorountine);
            }
        }

        if(Input.GetAxis("Horizontal") != 0)
        {
            isAccelerating = true;
            if(isMoving == false && canMove == true)
            {
                trigger.enabled = true;
                isMoving = true;
                canMove = false;
                hDirection = Input.GetAxisRaw("Horizontal");

                oldPosition = transform.position;
                activeCorountine = GoHorizontaly();
                StartCoroutine(activeCorountine);
            }
        }

        if(Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") == 0)
        {
            isAccelerating = false;
        }

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 11f;
        Vector3 worldMouse = Camera.main.ScreenToWorldPoint(mousePos);
        
        Vector3 turretOrientation = worldMouse - turret.position;
        turretOrientation.y = 0f;
        turret.forward = turretOrientation;

        if(isGrinding)
        {
            if(playerInContact.GetComponent<TestScript>().isGrinding == false)
            {
                isGrinding = false;
                activeCorountine = FinishMoving();
                StartCoroutine(activeCorountine);
                
                Debug.Log(this + " Won");
            }

            if(Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") == 0)
            {
                isGrinding = false;
                Debug.Log(this + " Gave up");
                GetComponent<PlayerImpact>().LooseThePosition();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            playerInContact = other.gameObject;
            trigger.enabled = false;
            canMove = false;

            if(activeCorountine != null)
                StopCoroutine(activeCorountine);

            if(isMoving == true && other.GetComponent<TestScript>().isMoving == false && isGrinding == false)
            {
                Debug.Log(other + " was hit away!");
                other.GetComponent<PlayerImpact>().GotHit();

                activeCorountine = FinishMoving();
                StartCoroutine(activeCorountine);
            }
            else if(isMoving == true && other.GetComponent<TestScript>().isMoving == true)
            {
                if(isAccelerating == false && other.GetComponent<TestScript>().isAccelerating == false)
                {
                    Debug.Log(this + " had a bounce!");

                    activeCorountine = MoveBackToOldPosition();
                    StartCoroutine(activeCorountine);
                }
                else if(isAccelerating == true && other.GetComponent<TestScript>().isAccelerating == true)
                {
                    Debug.Log(this + " is fighting back!");

                    isGrinding = true;
                }
            }
        }
    }

    IEnumerator GoVerticaly()
    {
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
        trigger.enabled = false;
        yield return new WaitForSeconds(0.5f);
        canMove = true;
        yield return null;
    }

    IEnumerator GoHorizontaly()
    {
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
        trigger.enabled = false;
        yield return new WaitForSeconds(0.5f);
        canMove = true;
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
        trigger.enabled = false;
        yield return new WaitForSeconds(0.5f);
        canMove = true;
        yield return null;
    }

    IEnumerator FinishMoving()
    {
        Debug.Log(newPosition);
        while(transform.position != newPosition)
        {
            float step = movementSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, newPosition, step);
            yield return null;
        }

        isMoving = false;
        trigger.enabled = false;
        yield return new WaitForSeconds(0.5f);
        canMove = true;
        yield return null;
    }
}
