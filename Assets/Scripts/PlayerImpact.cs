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

    private IEnumerator activeCorountine;

    void Update()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 toOther = player.position - transform.position;

        Debug.Log(this + " " + Vector3.Dot(forward.normalized, toOther.normalized));
    }

    public void GotHit()
    {
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

        // if(newPosition.x > 3.5f * xValue)
        //     newPosition.x = 3.5f * xValue;

        // if(newPosition.x < -3.5f * xValue)
        //     newPosition.x = -3.5f * xValue;

        while(transform.position != newPosition)
        {
            float step = 7f * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, newPosition, step);
            yield return null;
        }

        // isMoving = false;
        yield return null;
    }

    IEnumerator VerticalImpact()
    {
        newPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z + 3.5f * -vDirection);

        // if(newPosition.z > 3.5f * zValue)
        //     newPosition.z = 3.5f * zValue;

        // if(newPosition.z < -3.5f * zValue)
        //     newPosition.z = -3.5f * zValue;

        while(transform.position != newPosition)
        {
            float step = 7f * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, newPosition, step);
            yield return null;
        }

        // isMoving = false;
        yield return null;
    }

    IEnumerator HorizontalLoss()
    {
        newPosition = new Vector3(transform.position.x + 3.5f * 2f * -hDirection, transform.position.y, transform.position.z);

        // if(newPosition.x > 3.5f * xValue)
        //     newPosition.x = 3.5f * xValue;

        // if(newPosition.x < -3.5f * xValue)
        //     newPosition.x = -3.5f * xValue;

        while(transform.position != newPosition)
        {
            float step = 7f * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, newPosition, step);
            yield return null;
        }

        // isMoving = false;
        yield return null;
    }

    IEnumerator VerticalLoss()
    {
        newPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z + 3.5f * 2f * -vDirection);

        // if(newPosition.z > 3.5f * zValue)
        //     newPosition.z = 3.5f * zValue;

        // if(newPosition.z < -3.5f * zValue)
        //     newPosition.z = -3.5f * zValue;

        while(transform.position != newPosition)
        {
            float step = 7f * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, newPosition, step);
            yield return null;
        }

        // isMoving = false;
        yield return null;
    }
}
