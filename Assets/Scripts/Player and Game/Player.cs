using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public string playerName;
    public Color32 color;
    //an enum to keep track of what the player is doing. Would like to use it to control when the player is in menus and when the player can move
    public enum TurnState 
    { 
        BeforeRoll,
        Rolling,
        Moving,
        Landed
    }
    TurnState turnState;

    public int id;
    public int level = 1;
    
    PlayerMovement movement;
    // Start is called before the first frame update
    void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        movement.SetPlayer(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetMoved(Space space)
    {
        movement.TeleportToSpace(space);
    }

    public void RollDice(int maxNumber = 6)
    {
        movement.PlayerRollDice(maxNumber);
    }

    public void MakePlayerActive()
    {
        movement.enabled = true;
    }

    public void StopPlayerActive()
    {
        movement.enabled = false;
    }

}
