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
    
    PlayerMovement movement;

    public delegate void PlayerTurnStateChanged(Player player, TurnState turnState);
    public event PlayerTurnStateChanged OnPlayerTurnStateChanged;

    // Start is called before the first frame update
    void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        movement.SetPlayer(this);
    }

    public void MakePlayerActive()
    {
        movement.enabled = true;
        SetTurnState(TurnState.BeforeRoll);
    }

    public void StopPlayerActive()
    {
        movement.enabled = false;
    }

    public void StartRolling()
    {
        SetTurnState(TurnState.Rolling);
    }
    public void GetMoved(Space space)
    {
        movement.TeleportToSpace(space);
    }

    public int GetSpacesRemaining()
    {
        return movement.spacesToMove;
    }

    public TurnState GetTurnState()
    {
        return turnState;
    }

    public void SetTurnState(TurnState turnState)
    {
        this.turnState = turnState;
        OnPlayerTurnStateChanged?.Invoke(this, turnState);
    }
}
