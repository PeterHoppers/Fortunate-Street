using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// When the human player makes input, this file will filter their inputs into the player logic to perform the needed actions
/// </summary>
public class HumanController : Controller
{
    Player player;
    PlayerMovement playerMovement;

    private void Awake()
    {
        InitController();
        player = GetComponent<Player>();
        playerMovement = GetComponent<PlayerMovement>();        
    }

    private void OnSubmit()
    {
        TurnState turnState = player.GetTurnState();

        if (turnState == TurnState.Rolling)
        {
            player.PerformRoll();
        }
    }

    private void OnMove(InputValue value)
    {
        TurnState turnState = player.GetTurnState();

        if (turnState != TurnState.Moving)
        {
            return;
        }

        Vector2 movementInput = value.Get<Vector2>();

        if (movementInput == Vector2.left)
        {
            playerMovement.SelectSpace(SpaceDirection.Left);
        }
        else if (movementInput == Vector2.right)
        {
            playerMovement.SelectSpace(SpaceDirection.Right);
        }
        else if (movementInput == Vector2.up)
        {
            playerMovement.SelectSpace(SpaceDirection.Up);
        }
        else if (movementInput == Vector2.down)
        {
            playerMovement.SelectSpace(SpaceDirection.Down);
        }
    }
}
