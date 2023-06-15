using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerImpact : MonoBehaviour
{
    private int xValue;
    private int zValue;

    private float hDirection;
    private float vDirection;

    private Vector3 oldPosition;
    private Vector3 newPosition;

    public Transform player;

    [HideInInspector]
    public IEnumerator activeCorountine;

    private GameObject theOne;

    void Update()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 toOther = player.position - transform.position;
    }

    public void GotHit()
    {
        if(activeCorountine != null)
            StopCoroutine(activeCorountine);

        if(gameObject.GetComponent<TestScript>() == true)
        {
            gameObject.GetComponent<TestScript>().canMove = false;
            xValue = gameObject.GetComponent<TestScript>().xValue;
            zValue = gameObject.GetComponent<TestScript>().zValue;
        }
        else if(gameObject.GetComponent<PlayerController>() == true)
        {
            gameObject.GetComponent<PlayerController>().canMove = false;
            xValue = gameObject.GetComponent<PlayerController>().xValue;
            zValue = gameObject.GetComponent<PlayerController>().zValue;
        }

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        Vector3 opponent = player.position - transform.position;

        vDirection = Vector3.Dot(forward.normalized, opponent.normalized);
        hDirection = Vector3.Dot(right.normalized, opponent.normalized);

        if(hDirection == 1f || hDirection == -1f)
        {
            if(gameObject.GetComponent<TestScript>() == true)
            {
                activeCorountine = HorizontalImpact();
                StartCoroutine(activeCorountine);
            }
            else if(gameObject.GetComponent<PlayerController>() == true)
            {
                activeCorountine = PlayerHorizontalImpact();
                StartCoroutine(activeCorountine);
            }
        }

        if(vDirection == 1f || vDirection == -1f)
        {
            if(gameObject.GetComponent<TestScript>() == true)
            {
                activeCorountine = VerticalImpact();
                StartCoroutine(activeCorountine);
            }
            else if(gameObject.GetComponent<PlayerController>() == true)
            {
                activeCorountine = PlayerVerticalImpact();
                StartCoroutine(activeCorountine);
            }
        }
    }

    public void LooseThePosition()
    {
        if(activeCorountine != null)
            StopCoroutine(activeCorountine);

        if(gameObject.GetComponent<TestScript>() == true)
        {
            oldPosition = gameObject.GetComponent<TestScript>().oldPosition;
            gameObject.GetComponent<TestScript>().canMove = false;
            xValue = gameObject.GetComponent<TestScript>().xValue;
            zValue = gameObject.GetComponent<TestScript>().zValue;
        }
        else if(gameObject.GetComponent<PlayerController>() == true)
        {
            oldPosition = gameObject.GetComponent<PlayerController>().oldPosition;
            gameObject.GetComponent<PlayerController>().canMove = false;
            xValue = gameObject.GetComponent<PlayerController>().xValue;
            zValue = gameObject.GetComponent<PlayerController>().zValue;
        }

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        Vector3 opponent = player.position - transform.position;

        vDirection = Vector3.Dot(forward.normalized, opponent.normalized);
        hDirection = Vector3.Dot(right.normalized, opponent.normalized);

        if(hDirection == 1f || hDirection == -1f)
        {
            if(gameObject.GetComponent<TestScript>() == true)
            {
                activeCorountine = HorizontalLoss();
                StartCoroutine(activeCorountine);
            }
            else if(gameObject.GetComponent<PlayerController>() == true)
            {
                activeCorountine = PlayerHorizontalLoss();
                StartCoroutine(activeCorountine);
            }
        }

        if(vDirection == 1f || vDirection == -1f)
        {
            if(gameObject.GetComponent<TestScript>() == true)
            {
                activeCorountine = VerticalLoss();
                StartCoroutine(activeCorountine);
            }
            else if(gameObject.GetComponent<PlayerController>() == true)
            {
                activeCorountine = PlayerVerticalLoss();
                StartCoroutine(activeCorountine);
            }
        }
    }

    IEnumerator HorizontalImpact()
    {
        newPosition = new Vector3(transform.position.x + 3.5f * -hDirection, transform.position.y, transform.position.z);

        while(transform.position != newPosition)
        {
            float step = 7f * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, newPosition, step);
            yield return null;
        }

        if(newPosition.x > 3.5f * xValue)
        {
            newPosition.x = 3.5f * xValue;

            gameObject.GetComponent<TestScript>().trigger.enabled = true;
            gameObject.GetComponent<TestScript>().isMoving = true;
        }

        if(newPosition.x < -3.5f * xValue)
        {
            newPosition.x = -3.5f * xValue;

            gameObject.GetComponent<TestScript>().trigger.enabled = true;
            gameObject.GetComponent<TestScript>().isMoving = true;
        }

        while(transform.position != newPosition)
        {
            float step = 7f * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, newPosition, step);
            yield return null;
        }

        gameObject.GetComponent<TestScript>().isMoving = false;
        gameObject.GetComponent<TestScript>().trigger.enabled = false;

        yield return new WaitForSeconds(0.5f);

        gameObject.GetComponent<TestScript>().canMove = true;
        
        yield return null;
    }

    IEnumerator PlayerHorizontalImpact()
    {
        newPosition = new Vector3(transform.position.x + 3.5f * -hDirection, transform.position.y, transform.position.z);

        while(transform.position != newPosition)
        {
            float step = 7f * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, newPosition, step);
            yield return null;
        }

        if(newPosition.x > 3.5f * xValue)
        {
            newPosition.x = 3.5f * xValue;

            gameObject.GetComponent<PlayerController>().trigger.enabled = true;
            gameObject.GetComponent<PlayerController>().isMoving = true;
            gameObject.GetComponent<PlayerController>().isReturning = true;
        }

        if(newPosition.x < -3.5f * xValue)
        {
            newPosition.x = -3.5f * xValue;

            gameObject.GetComponent<PlayerController>().trigger.enabled = true;
            gameObject.GetComponent<PlayerController>().isMoving = true;
            gameObject.GetComponent<PlayerController>().isReturning = true;
        }

        while(transform.position != newPosition)
        {
            float step = 7f * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, newPosition, step);
            yield return null;
        }

        gameObject.GetComponent<PlayerController>().isMoving = false;
        gameObject.GetComponent<PlayerController>().trigger.enabled = false;
        gameObject.GetComponent<PlayerController>().isReturning = false;

        yield return new WaitForSeconds(0.5f);

        gameObject.GetComponent<PlayerController>().canMove = true;

        yield return null;
    }

    IEnumerator PlayerVerticalImpact()
    {
        newPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z + 3.5f * -vDirection);

        while(transform.position != newPosition)
        {
            float step = 7f * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, newPosition, step);
            yield return null;
        }

        if(newPosition.z > 3.5f * zValue)
        {
            newPosition.z = 3.5f * zValue;

            gameObject.GetComponent<PlayerController>().trigger.enabled = true;
            gameObject.GetComponent<PlayerController>().isMoving = true;
            gameObject.GetComponent<PlayerController>().isReturning = true;
        }

        if(newPosition.z < -3.5f * zValue)
        {
            newPosition.z = -3.5f * zValue;

            gameObject.GetComponent<PlayerController>().trigger.enabled = true;
            gameObject.GetComponent<PlayerController>().isMoving = true;
            gameObject.GetComponent<PlayerController>().isReturning = true;
        }

        while(transform.position != newPosition)
        {
            float step = 7f * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, newPosition, step);
            yield return null;
        }

        gameObject.GetComponent<PlayerController>().isMoving = false;
        gameObject.GetComponent<PlayerController>().trigger.enabled = false;
        gameObject.GetComponent<PlayerController>().isReturning = false;

        yield return new WaitForSeconds(0.5f);

        gameObject.GetComponent<PlayerController>().canMove = true;

        yield return null;
    }

    IEnumerator VerticalImpact()
    {
        newPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z + 3.5f * -vDirection);

        while(transform.position != newPosition)
        {
            float step = 7f * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, newPosition, step);
            yield return null;
        }

        if(newPosition.z > 3.5f * zValue)
        {
            newPosition.z = 3.5f * zValue;

            if(gameObject.GetComponent<TestScript>() == true)
            {
                gameObject.GetComponent<TestScript>().trigger.enabled = true;
                gameObject.GetComponent<TestScript>().isMoving = true;
            }

            if(gameObject.GetComponent<PlayerController>() == true)
            {
                gameObject.GetComponent<PlayerController>().trigger.enabled = true;
                gameObject.GetComponent<PlayerController>().isMoving = true;
            }
        }

        if(newPosition.z < -3.5f * zValue)
        {
            newPosition.z = -3.5f * zValue;

            if(gameObject.GetComponent<TestScript>() == true)
            {
                gameObject.GetComponent<TestScript>().trigger.enabled = true;
                gameObject.GetComponent<TestScript>().isMoving = true;
            }

            if(gameObject.GetComponent<PlayerController>() == true)
            {
                gameObject.GetComponent<PlayerController>().trigger.enabled = true;
                gameObject.GetComponent<PlayerController>().isMoving = true;
            }
        }

        while(transform.position != newPosition)
        {
            float step = 7f * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, newPosition, step);
            yield return null;
        }

        if(gameObject.GetComponent<TestScript>() == true)
            gameObject.GetComponent<TestScript>().isMoving = false;

        if(gameObject.GetComponent<PlayerController>() == true)
            gameObject.GetComponent<PlayerController>().isMoving = false;

        yield return new WaitForSeconds(0.5f);

        if(gameObject.GetComponent<TestScript>() == true)
            gameObject.GetComponent<TestScript>().canMove = true;

        if(gameObject.GetComponent<PlayerController>() == true)
            gameObject.GetComponent<PlayerController>().canMove = true;

        yield return null;
    }

    IEnumerator HorizontalLoss()
    {
        newPosition = new Vector3(oldPosition.x + 3.5f * -hDirection , oldPosition.y, oldPosition.z);

        while(transform.position != newPosition)
        {
            float step = 7f * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, newPosition, step);
            yield return null;
        }

        if(gameObject.name == "TestOpponent")
            gameObject.GetComponent<TestScript>().isMoving = false;

        if(gameObject.name == "Player")
            gameObject.GetComponent<PlayerController>().isMoving = false;
        
        yield return new WaitForSeconds(0.5f);

        if(gameObject.GetComponent<TestScript>() == true)
            gameObject.GetComponent<TestScript>().canMove = true;

        if(gameObject.GetComponent<PlayerController>() == true)
            gameObject.GetComponent<PlayerController>().canMove = true;

        yield return null;
    }
    
    IEnumerator VerticalLoss()
    {
        newPosition = new Vector3(oldPosition.x, oldPosition.y, oldPosition.z + 3.5f * -vDirection);

        while(transform.position != newPosition)
        {
            float step = 7f * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, newPosition, step);
            yield return null;
        }

        if(gameObject.name == "TestOpponent")
            gameObject.GetComponent<TestScript>().isMoving = false;

        if(gameObject.name == "Player")
            gameObject.GetComponent<PlayerController>().isMoving = false;

        yield return new WaitForSeconds(0.5f);
        
        if(gameObject.GetComponent<TestScript>() == true)
            gameObject.GetComponent<TestScript>().canMove = true;

        if(gameObject.GetComponent<PlayerController>() == true)
            gameObject.GetComponent<PlayerController>().canMove = true;

        yield return null;
    }

    IEnumerator PlayerHorizontalLoss()
    {
        newPosition = new Vector3(oldPosition.x + 3.5f * -hDirection , oldPosition.y, oldPosition.z);

        while(transform.position != newPosition)
        {
            float step = 7f * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, newPosition, step);
            yield return null;
        }

        if(newPosition.x > 3.5f * xValue)
        {
            newPosition.x = 3.5f * xValue;

            gameObject.GetComponent<PlayerController>().trigger.enabled = true;
            gameObject.GetComponent<PlayerController>().isMoving = true;
            gameObject.GetComponent<PlayerController>().isReturning = true;
        }

        if(newPosition.x < -3.5f * xValue)
        {
            newPosition.x = -3.5f * xValue;

            gameObject.GetComponent<PlayerController>().trigger.enabled = true;
            gameObject.GetComponent<PlayerController>().isMoving = true;
            gameObject.GetComponent<PlayerController>().isReturning = true;
        }

        while(transform.position != newPosition)
        {
            float step = 7f * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, newPosition, step);
            yield return null;
        }

        gameObject.GetComponent<PlayerController>().isMoving = false;
        gameObject.GetComponent<PlayerController>().trigger.enabled = false;
        gameObject.GetComponent<PlayerController>().isReturning = false;
        
        yield return new WaitForSeconds(0.5f);

        gameObject.GetComponent<PlayerController>().canMove = true;

        yield return null;
    }

    IEnumerator PlayerVerticalLoss()
    {
        newPosition = new Vector3(oldPosition.x, oldPosition.y, oldPosition.z + 3.5f * -vDirection);

        while(transform.position != newPosition)
        {
            float step = 7f * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, newPosition, step);
            yield return null;
        }

        if(newPosition.z > 3.5f * zValue)
        {
            newPosition.z = 3.5f * zValue;

            gameObject.GetComponent<PlayerController>().trigger.enabled = true;
            gameObject.GetComponent<PlayerController>().isMoving = true;
            gameObject.GetComponent<PlayerController>().isReturning = true;
        }

        if(newPosition.z < -3.5f * zValue)
        {
            newPosition.z = -3.5f * zValue;

            gameObject.GetComponent<PlayerController>().trigger.enabled = true;
            gameObject.GetComponent<PlayerController>().isMoving = true;
            gameObject.GetComponent<PlayerController>().isReturning = true;
        }

        while(transform.position != newPosition)
        {
            float step = 7f * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, newPosition, step);
            yield return null;
        }

        gameObject.GetComponent<PlayerController>().isMoving = false;
        gameObject.GetComponent<PlayerController>().trigger.enabled = false;
        gameObject.GetComponent<PlayerController>().isReturning = false;

        yield return new WaitForSeconds(0.5f);

        gameObject.GetComponent<PlayerController>().canMove = true;

        yield return null;
    }
}
