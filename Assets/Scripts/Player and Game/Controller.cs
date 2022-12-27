using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Controller : MonoBehaviour
{
    PlayerInput playerInput;

    //Current Control Scheme
    private string currentControlScheme;

    public void InitController()
    {
        playerInput = GetComponent<PlayerInput>();
        currentControlScheme = playerInput.currentControlScheme;
    }

    public InputActionAsset GetActionAsset()
    {
        return playerInput.actions;
    }

    public PlayerInput GetPlayerInput()
    {
        return playerInput;
    }
}
