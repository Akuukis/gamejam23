using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public float movementSpeed = 5f;

    [Header("Grid Size")]
    public int xValue = 1;
    public int zValue = 1;

    public float impactDelay = 0.5f;

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

    private IEnumerator activeCorountine;
    private GameObject playerInContact;

    void Update()
    {
        if(Input.GetKey(KeyCode.O) == true)
        {
            isAccelerating = true;
            if(isMoving == false && canMove == true)
            {
                trigger.enabled = true;
                isMoving = true;
                canMove = false;

                activeCorountine = GoVerticaly(-1f);
                // activeCorountine = GoHorizontaly(-1f);
                StartCoroutine(activeCorountine);
            }
        }

        if(Input.GetKey(KeyCode.P) == true)
        {
            isAccelerating = true;
            if(isMoving == false && canMove == true)
            {
                trigger.enabled = true;
                isMoving = true;
                canMove = false;

                activeCorountine = GoVerticaly(1f);
                // activeCorountine = GoHorizontaly(1f);
                StartCoroutine(activeCorountine);
            }
        }

        if(Input.GetKey(KeyCode.O) == false && Input.GetKey(KeyCode.P) == false)
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
            if(playerInContact.GetComponent<PlayerController>().isGrinding == false)
            {
                isGrinding = false;
                activeCorountine = FinishMoving();
                StartCoroutine(activeCorountine);
                
                Debug.Log(this + " Won");
            }

            if(Input.GetKey(KeyCode.O) == false && Input.GetKey(KeyCode.P) == false)
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
            // trigger.enabled = false;
            canMove = false;

            playerInContact = other.gameObject;

            if(activeCorountine != null)
                StopCoroutine(activeCorountine);

            if(isMoving == true && other.GetComponent<PlayerController>().isMoving == false && isGrinding == false)
            {
                Debug.Log(other + " was hit away!");
                other.GetComponent<PlayerImpact>().GotHit();

                activeCorountine = FinishMoving();
                StartCoroutine(activeCorountine);
            }
            else if(isMoving == true && other.GetComponent<PlayerController>().isMoving == true)
            {
                if(isAccelerating == false && other.GetComponent<PlayerController>().isAccelerating == false)
                {
                    Debug.Log(this + " had a bounce!");

                    activeCorountine = MoveBackToOldPosition();
                    StartCoroutine(activeCorountine);
                }
                else if(isAccelerating == true && other.GetComponent<PlayerController>().isAccelerating == true)
                {
                    Debug.Log(this + " is fighting back!");

                    isGrinding = true;
                }
            }
        }
    }

    IEnumerator GoVerticaly(float xd)
    {
        oldPosition = transform.position;
        newPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z + 3.5f * xd);

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
        yield return new WaitForSeconds(impactDelay);
        canMove = true;
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
        trigger.enabled = false;
        yield return new WaitForSeconds(impactDelay);
        canMove = true;
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
        trigger.enabled = false;
        yield return new WaitForSeconds(impactDelay);
        canMove = true;
        yield return null;
    }

    IEnumerator FinishMoving()
    {
        while(transform.position != newPosition)
        {
            float step = movementSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, newPosition, step);
            yield return null;
        }

        isMoving = false;
        trigger.enabled = false;
        yield return new WaitForSeconds(impactDelay);
        canMove = true;
        yield return null;
    }
}
