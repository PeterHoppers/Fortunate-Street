using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropertyBuyUI : LandedUIBaseSpace
{
    Space space;
    Player player;

    //TODO: Different UI for buying a space vs. landing on someone else's space
    public override void SetUpBaseSpace(Player player, Space space)
    {
        this.player = player;
        this.space = space;
        Debug.Log($"The {player.playerName} has landed on the property {space.name}");
        //player.SetTurnState(TurnState.Finished);
    }

    public void BuySpace()
    {
        space.GetComponent<Property>().BuySpace(player);
        player.SetTurnState(TurnState.Finished);
    }

    public void EndTurn()
    { 
        player.SetTurnState(TurnState.Finished);
    }
}
