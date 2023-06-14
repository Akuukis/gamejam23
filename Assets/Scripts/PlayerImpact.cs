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

    void Update()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 toOther = player.position - transform.position;
    }

    public void GotHit()
    {
        if(gameObject.GetComponent<TestScript>() == true)
        {
            gameObject.GetComponent<TestScript>().canMove = false;
        }
        else if(gameObject.GetComponent<PlayerController>() == true)
        {
            gameObject.GetComponent<PlayerController>().canMove = false;
        }

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        Vector3 opponent = player.position - transform.position;

        vDirection = Vector3.Dot(forward.normalized, opponent.normalized);
        hDirection = Vector3.Dot(right.normalized, opponent.normalized);

        if(hDirection == 1f || hDirection == -1f)
        {
            activeCorountine = HorizontalImpact();
            StartCoroutine(activeCorountine);
        }

        if(vDirection == 1f || vDirection == -1f)
        {
            activeCorountine = VerticalImpact();
            StartCoroutine(activeCorountine);
        }
    }

    public void LooseThePosition()
    {
        if(gameObject.name == "TestOpponent")
        {
            Debug.Log("Test");
            oldPosition = gameObject.GetComponent<TestScript>().oldPosition;
        }

        if(gameObject.name == "Player")
        {
            Debug.Log("Player");
            oldPosition = gameObject.GetComponent<PlayerController>().oldPosition;
        }

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        Vector3 opponent = player.position - transform.position;

        vDirection = Vector3.Dot(forward.normalized, opponent.normalized);
        hDirection = Vector3.Dot(right.normalized, opponent.normalized);

        if(hDirection == 1f || hDirection == -1f)
        {
            activeCorountine = HorizontalLoss();
            StartCoroutine(activeCorountine);
        }

        if(vDirection == 1f || vDirection == -1f)
        {
            activeCorountine = VerticalLoss();
            StartCoroutine(activeCorountine);
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

        if(gameObject.GetComponent<TestScript>() == true)
            gameObject.GetComponent<TestScript>().isMoving = false;

        if(gameObject.GetComponent<PlayerController>() == true)
            gameObject.GetComponent<PlayerController>().isMoving = false;

        yield return new WaitForSeconds(1f);
        
        if(gameObject.GetComponent<TestScript>() == true)
            gameObject.GetComponent<TestScript>().canMove = true;

        if(gameObject.GetComponent<PlayerController>() == true)
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

        if(gameObject.name == "TestOpponent")
        {
            gameObject.GetComponent<TestScript>().isMoving = false;
            // gameObject.GetComponent<TestScript>().canMove = true;
        }

        if(gameObject.name == "Player")
        {
            gameObject.GetComponent<PlayerController>().isMoving = false;
            // gameObject.GetComponent<PlayerController>().canMove = true;
        }

        yield return new WaitForSeconds(1f);
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
        
        yield return new WaitForSeconds(1f);

        if(gameObject.name == "TestOpponent")
            // gameObject.GetComponent<TestScript>().canMove = true;

        if(gameObject.name == "Player")
            // gameObject.GetComponent<PlayerController>().canMove = true;

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
        {
            gameObject.GetComponent<TestScript>().isMoving = false;
            // gameObject.GetComponent<TestScript>().canMove = true;
        }

        if(gameObject.name == "Player")
        {
            gameObject.GetComponent<PlayerController>().isMoving = false;
            // gameObject.GetComponent<PlayerController>().canBeDamaged = true;
        }

        yield return new WaitForSeconds(1f);
        yield return null;
    }
}
