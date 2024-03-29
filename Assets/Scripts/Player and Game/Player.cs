using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public string playerName;
    public Color32 color;
    public int id;

    TurnState turnState;
   
    public int level = 1;
    int amountToRoll = 0;
    
    protected PlayerMovement movement;
    public PlayerInput input;
    public Controller controller;

    public delegate void PlayerTurnStateChanged(TurnState turnState);
    public event PlayerTurnStateChanged OnPlayerTurnStateChanged;
    public delegate void PlayerRolled(Player player, int spaces);
    public event PlayerRolled OnPlayerRolled;

    // Start is called before the first frame update
    void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        movement.SetPlayer(this);
    }

    public void SetPlayerName(string playerName)
    {
        this.playerName = playerName;
    }

    public void SetPlayerColor(Color color)
    {
        this.color = color;
    }

    public void SetPlayerIndex(int id)
    {
        this.id = id;
    }

    /// <summary>
    /// Called by Game Manager to set up this player for its upcoming turn
    /// </summary>
    public void MakePlayerActive()
    {       
        SetTurnState(TurnState.BeforeRoll);
        input.ActivateInput();
    }

    public void DisablePlayer()
    {
        if (input == null)
        {
            input = gameObject.GetComponent<PlayerInput>();
        }
        input.DeactivateInput();
    }

    /// <summary>
    /// Called currently through the "Roll Dice" button from the UI Mananger
    /// This is a call here, since there are other places that might allow the player to start rolling, like chance spaces or the roll again space
    /// </summary>
    public void StartRolling(int amountToRoll = 6)
    {
        SetTurnState(TurnState.Rolling);
        this.amountToRoll = amountToRoll;
    }

    public void PerformRoll()
    {
        AlertRoll(Random.Range(1, amountToRoll + 1));
    }

    public void AlertRoll(int amountRolled)
    {
        OnPlayerRolled?.Invoke(this, amountRolled);
    }

    /// <summary>
    /// Attached to the event of the dice being rolled.
    /// </summary>
    /// <param name="player"></param>
    /// <param name="spaces"></param>
    public void StartMoving(int spaces)
    {
        movement.enabled = true;
        movement.spacesToMove = spaces;
        movement.SetupTurnMovement();
        SetTurnState(TurnState.Moving);
    }

    /// <summary>
    /// Called whenever the player runs out of spaces to move
    /// </summary>
    public void StoppedMoving()
    {
        SetTurnState(TurnState.Stopped);
    }

    /// <summary>
    /// Called whenever a player lands and then decides to go back and move
    /// </summary>
    public void ContinueMoving()
    {
        movement.ReverseMovement();
        movement.SetupMovementOptions();
        SetTurnState(TurnState.Moving);
    }

    /// <summary>
    /// Called when a player chooses to land on the space that they've landed on
    /// </summary>
    public void EndMoving()
    {
        movement.EndMovement();
        movement.enabled = false;
        SetTurnState(TurnState.Landed);
    }

    /// <summary>
    /// The following two methods are called to prevent the player from moving while there is a pop-up on screen
    /// </summary>
    public void PauseMoving()
    {
        movement.enabled = false;
    }

    public void UnPauseMoving()
    {
        movement.enabled = true;
    }

    public int GetSpacesLeftToMove()
    {
        return movement.spacesToMove;
    }

    public Space GetCurrentSpace()
    {
        return movement.currentSpace;
    }

    public void GetMoved(Space space, bool hasLanded = true)
    {
        movement.TeleportToSpace(space, hasLanded);
    }

    public TurnState GetTurnState()
    {
        return turnState;
    }

    protected void SetTurnState(TurnState turnState)
    {
        this.turnState = turnState;
        OnPlayerTurnStateChanged?.Invoke(turnState);
        Debug.Log($"{playerName} is now in the {turnState}.");
    }
}
