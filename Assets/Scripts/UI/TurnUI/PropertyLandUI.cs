using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropertyLandUI : LandedUIBaseSpace
{
    public UIManager manager;
    public GameObject purchaseUI;
    public GameObject investUI;
    //public GameObject buyOutUI;

    //info here saved so that UI can reference them
    Property space;
    Player player;

    //TODO: Different UI for buying a space vs. landing on someone else's space
    public override void SetUpUI(Player player, Space space)
    {
        this.player = player;
        this.space = space.GetComponent<Property>();

        //TODO: Find a way so that the manager doesn't need to be brought in here. Or, find a better way to toggle this panel
        manager.ToggleMoneyDisplay(player);

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

        Debug.Log($"{player.playerName} has landed on the property {space.name}");
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

    public override void TurnOffUI()
    {
        throw new System.NotImplementedException();
    }
}
