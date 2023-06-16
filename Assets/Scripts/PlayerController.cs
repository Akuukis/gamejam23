using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
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
    public bool isReturning = false;
    
    public Transform turret;
    public Collider col;
    public Collider trigger;

    [HideInInspector]
    public Vector3 oldPosition;
    private Vector3 newPosition;

    public IEnumerator activeCorountine;
    private GameObject playerInContact;

    private Vector2 movementInput = Vector2.zero;
    private Vector2 rotationInput = Vector2.zero;
    private bool threw = false;

    public PlayerInput playerInput;

    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    public void SetCursor(InputAction.CallbackContext context)
    {
        rotationInput = context.ReadValue<Vector2>();
    }

    public void OnAction(InputAction.CallbackContext context)
    {
        threw = context.ReadValue<bool>();
        threw = context.action.triggered;
    }

    void Start()
    {
        playerInput = gameObject.GetComponent<PlayerInput>();
        Debug.Log(playerInput.playerIndex);
        // Debug.Log(InputDevice.all[playerInput.playerIndex]);
        // Debug.Log(InputSystem.devices[playerInput.playerIndex]);

        Debug.Log(this + playerInput.currentControlScheme);
    }

    void Update()
    {    
        if(Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            if(playerInput.currentControlScheme == "Keyboard")
            {
                Vector3 mousePos = Input.mousePosition;
                mousePos.z = 11f;
                Vector3 worldMouse = Camera.main.ScreenToWorldPoint(mousePos);
                
                Vector3 turretOrientation = worldMouse - turret.position;
                turretOrientation.y = 0f;
                turret.forward = turretOrientation;
            }
        }
        else if(rotationInput.x != 0 || rotationInput.y != 0)
        {
            Vector3 lookDirection = new Vector3(rotationInput.x, 0, rotationInput.y);
            turret.transform.rotation = Quaternion.LookRotation(lookDirection);
        }

        // if(playerInput.currentControlScheme == "Keyboard")
        // {
        //     // Cursor.visible = true;
            
        //     Vector3 mousePos = Input.mousePosition;
        //     mousePos.z = 11f;
        //     Vector3 worldMouse = Camera.main.ScreenToWorldPoint(mousePos);
            
        //     Vector3 turretOrientation = worldMouse - turret.position;
        //     turretOrientation.y = 0f;
        //     turret.forward = turretOrientation;
        // }
        // else if(playerInput.currentControlScheme == "Gamepad")
        // {
        //     Cursor.visible = false;
        //     Vector3 lookDirection = new Vector3(rotationInput.x, 0, rotationInput.y);
        //     turret.transform.rotation = Quaternion.LookRotation(lookDirection);
        // }

        if(canMove == true)
        {
            if(movementInput.y != 0 && isMoving == false)
            {
                trigger.enabled = true;
                isMoving = true;
                canMove = false;
                vDirection = movementInput.y;
                // vDirection = Input.GetAxisRaw("Vertical");

                oldPosition = transform.position;
                activeCorountine = GoVerticaly();
                StartCoroutine(activeCorountine);
            }

            if(movementInput.y == 0)
            {
                if(movementInput.x != 0 && isMoving == false)
                {
                    trigger.enabled = true;
                    isMoving = true;
                    canMove = false;
                    hDirection = movementInput.x;
                    // hDirection = Input.GetAxisRaw("Horizontal");

                    oldPosition = transform.position;
                    activeCorountine = GoHorizontaly();
                    StartCoroutine(activeCorountine);
                }
            }
        }

        if(movementInput.x != 0 || movementInput.y != 0)
        {
            if(canMove == false)
                isAccelerating = true;
        }

        if(movementInput.x == 0 && movementInput.y == 0)
        {
            isAccelerating = false;
        }

        if(isGrinding)
        {
            if(playerInContact.GetComponent<PlayerController>().isGrinding == false)
            {
                isGrinding = false;
                activeCorountine = FinishMoving();
                StartCoroutine(activeCorountine);
                
                Debug.Log(this + " Won");
            }

            if(movementInput.x == 0 && movementInput.y == 0)
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
            other.GetComponent<PlayerImpact>().player = gameObject.transform;
            gameObject.GetComponent<PlayerImpact>().player = other.transform;

            playerInContact = other.gameObject;
            // trigger.enabled = false;
            canMove = false;

            if(activeCorountine != null)
                StopCoroutine(activeCorountine);

            if(isMoving == true && other.GetComponent<PlayerController>().isMoving == false && isGrinding == false)
            {
                Debug.Log(other + " was hit away!");
                other.GetComponent<PlayerImpact>().GotHit();

                if(isReturning == false)
                {
                    activeCorountine = FinishMoving();
                    StartCoroutine(activeCorountine);
                }
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
        yield return new WaitForSeconds(impactDelay);
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
        yield return new WaitForSeconds(impactDelay);
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
        yield return new WaitForSeconds(impactDelay);
        canMove = true;
        yield return null;
    }

    IEnumerator FinishMoving()
    {
        Debug.Log("Won the fight");
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
