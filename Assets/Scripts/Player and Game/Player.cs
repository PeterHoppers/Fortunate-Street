using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public string Name;
    //an enum to keep track of what the player is doing
    public enum TurnState 
    { 
        BeforeRoll,
        Rolling,
        Moving,
        Landed
    }
    TurnState turnState;

    int id;
    int level;
    
    PlayerMovement movement;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetMoved(Space space)
    {
        movement.currentSpace = space;
    }

}