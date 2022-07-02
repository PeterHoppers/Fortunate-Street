using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public string playerName;
    public Color32 color;
    public int id;

    TurnState turnState;
   
    public int level = 1;
    
    protected PlayerMovement movement;
    protected Dice dice;

    public delegate void PlayerTurnStateChanged(TurnState turnState);
    public event PlayerTurnStateChanged OnPlayerTurnStateChanged;

    void OnEnable()
    {
        Dice.OnPlayerRolled += StartMoving;
    }

    // Update is called once per frame
    void OnDisable()
    {
        Dice.OnPlayerRolled -= StartMoving;
    }

    // Start is called before the first frame update
    void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        movement.SetPlayer(this);
        dice = Camera.main.GetComponent<Dice>();
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
    }

    /// <summary>
    /// Called currently through the "Roll Dice" button from the UI Mananger
    /// This is a call here, since there are other places that might allow the player to start rolling, like chance spaces or the roll again space
    /// </summary>
    public void StartRolling()
    {
        SetTurnState(TurnState.Rolling);
        //possibly change logic here depending on items used
        dice.SetupRoll(this, 6);
    }

    /// <summary>
    /// Attached to the event of the dice being rolled.
    /// </summary>
    /// <param name="player"></param>
    /// <param name="spaces"></param>
    protected void StartMoving(Player player, int spaces)
    {
        if (player != this)
        {
            return;
        }

        movement.enabled = true;
        movement.spacesToMove = spaces;
        movement.SetupMovementOptions();
        SetTurnState(TurnState.Moving);
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
    }

    public int GetSpacesLeftToMove()
    {
        return movement.spacesToMove;
    }

    public Space GetCurrentSpace()
    {
        return movement.currentSpace;
    }

    public void GetMoved(Space space)
    {
        movement.TeleportToSpace(space);
    }

    public TurnState GetTurnState()
    {
        return turnState;
    }

    public void SetTurnState(TurnState turnState)
    {
        this.turnState = turnState;
        OnPlayerTurnStateChanged?.Invoke(turnState);
        Debug.Log($"{playerName} is now in the {turnState}.");
    }
}
