using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropertyLandUI : LandedUIBaseSpace
{
    public GameObject purchaseUI;
    public GameObject investUI;
    //public GameObject buyOutUI;

    Property space;
    Player player;

    //TODO: Different UI for buying a space vs. landing on someone else's space
    public override void SetUpBaseSpace(Player player, Space space)
    {
        this.player = player;
        this.space = space.GetComponent<Property>();

        if (this.space == null)
        {
            Debug.LogError("Didn't pass in a property to the PropertyLandUI.");
        }

        //if the space has not been bought, give the player the prompt to buy it
        //else, if the player owns the space, give them the option to invest in it
        //if neither true, skip for now
        if (this.space.owner == null)
        {
            ActiveUI(purchaseUI);
        }
        else if (this.space.owner == player)
        {
            ActiveUI(investUI);
        }
        else //TODO: buyout logic
        {
            player.SetTurnState(TurnState.Finished);
        }

        Debug.Log($"The {player.playerName} has landed on the property {space.name}");
        //player.SetTurnState(TurnState.Finished);
    }

    public void BuySpace()
    {
        space.GetComponent<Property>().BuySpace(player);
    }

    public void EndTurn()
    { 
        player.SetTurnState(TurnState.Finished);
    }

    void ActiveUI(GameObject uiToTurnOn = null)
    {
        purchaseUI.SetActive(false);
        investUI.SetActive(false);
        //buyOutUI.SetActive(false);

        if (uiToTurnOn != null)
        {
            uiToTurnOn.SetActive(true);
        }
    }
}
