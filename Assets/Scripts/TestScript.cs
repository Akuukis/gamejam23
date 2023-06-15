using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestScript : MonoBehaviour
{
    public PlayerInputManager pim;

    public Transform spawn1;
    public Transform spawn2;

    private Vector3 spawn = new Vector3(-3.5f, 0, 0);

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
            pim.JoinPlayer();
    }

    private void OnPlayerJoined(PlayerInput playerInput)
    {
        Debug.Log("New PLayer");
        SetPlayerTransform(playerInput.transform);
    }

    void SetPlayerTransform(Transform player)
    {
        player.position = spawn;
        spawn = new Vector3(7, 0, 0);
    }
}
