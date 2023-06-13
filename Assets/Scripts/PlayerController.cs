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
    public bool isInImpact = false;

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
        if(canMove)
        {
            if(Input.GetAxis("Vertical") != 0 && isMoving == false)
            {
                isMoving = true;
                vDirection = Input.GetAxisRaw("Vertical");

                activeCorountine = GoVerticaly();
                StartCoroutine(activeCorountine);
            }

            if(Input.GetAxis("Horizontal") != 0 && isMoving == false)
            {
                isMoving = true;
                hDirection = Input.GetAxisRaw("Horizontal");

                activeCorountine = GoHorizontaly();
                StartCoroutine(activeCorountine);
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
        // else
        // {
        //     if(!isInImpact)
        //     {
        //         float time = Mathf.PingPong(Time.time * movementSpeed, 1);
        //         this.transform.position = Vector3.Lerp(new Vector3(3.5f, this.transform.position.y, -3.5f), new Vector3 (-3.5f, this.transform.position.y, -3.5f), time);
        //     }
        // }

        if(isInImpact)
        {
            if(Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") == 0)
            {
                isInImpact = false;
                Debug.Log("ASDF!");
                StartCoroutine(MoveBackToOldPosition());
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && other.GetComponent<PlayerController>().isMoving == false)
        {
            other.GetComponent<PlayerImpact>().GotHit();
            col.enabled = false;
        }

        if(other.tag == "Player" && other.GetComponent<PlayerController>().isMoving == true)
        {
            StopCoroutine(activeCorountine);
            isInImpact = true;
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

        while(transform.position != newPosition && isInImpact == false)
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
        oldPosition = transform.position;
        newPosition = new Vector3(transform.position.x + 3.5f * hDirection, transform.position.y, transform.position.z);

        if(newPosition.x > 3.5f * xValue)
            newPosition.x = 3.5f * xValue;

        if(newPosition.x < -3.5f * xValue)
            newPosition.x = -3.5f * xValue;

        while(transform.position != newPosition && isInImpact == false)
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
}
