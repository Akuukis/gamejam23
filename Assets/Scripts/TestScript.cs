using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class TestScript : MonoBehaviour
{
    public PlayerInputManager pim;

    private Vector3 spawn;

    private string playerOne;
    private string playerTwo;

    private InputDevice playerOneDevice;
    private InputDevice playerTwoDevice;

    [Header("Player Prefabs")]
    public GameObject playerOneModel;
    public GameObject playerTwoModel;

    public GameObject[] buttons;
    public GameObject[] players;

    void Start()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(buttons[0]);
    }

    public void CycleButtons(int i)
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(buttons[i]);
    }

    public void GetPlayers()
    {
        spawn = new Vector3(-3.5f, 0, 0);
        gameObject.GetComponent<PlayerInputManager>().playerPrefab = playerOneModel;
        pim.JoinPlayer();
        spawn = new Vector3(3.5f, 0, 0);
        gameObject.GetComponent<PlayerInputManager>().playerPrefab = playerTwoModel;
        pim.JoinPlayer();
    }

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        SetPlayerTransform(playerInput.transform);
        
        // PlayerInput.all[0].SwitchCurrentControlScheme("Gamepad", Gamepad.current);
        PlayerInput.all[0].SwitchCurrentControlScheme(playerOne, playerOneDevice);
        PlayerInput.all[1].SwitchCurrentControlScheme(playerTwo, playerTwoDevice);

        if(PlayerInput.all[0].currentControlScheme == "Gamepad" && PlayerInput.all[1].currentControlScheme == "Gamepad")
            Cursor.visible = false;
    }

    public void RestartGame()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject player in players)
            Destroy(player);

        GetPlayers();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    void SetPlayerTransform(Transform player)
    {
        player.position = spawn;
    }

    public void SetControls(int controllerMode)
    {
        switch(controllerMode)
        {
            case 4:
                // If player one chooses to use keyboard, automatically assign the controller to player two.
                playerOne = "Keyboard";
                playerOneDevice = Keyboard.current;


                playerTwo = "Gamepad";
                if(Gamepad.all[0] != null)
                    playerTwoDevice = Gamepad.all[0];
                // else
                //     playerTwoDevice = Gamepad.all[1];

                break;
            case 3:
                playerOne = "Gamepad";
                playerOneDevice = Gamepad.all[0];
                break;
            case 2:
                playerTwo = "Keyboard";
                playerTwoDevice = Keyboard.current;
                break;
            case 1:
                playerTwo = "Gamepad";
                if(playerOneDevice == Gamepad.all[0])
                    playerTwoDevice = Gamepad.all[1];
                // else
                //     playerTwoDevice = Gamepad.all[0];
                break;
        }
    }
}
