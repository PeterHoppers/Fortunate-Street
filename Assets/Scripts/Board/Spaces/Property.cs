using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Property : BuyableSpace
{
    public double shopPrice;
    public double investmentAmt;

    public void ChangePrice(double percentage)
    {
        //when called, change the shop price, shop value, and investiment amount, if applicatable
    }

    public override void PlayerLanded(Player player)
    {
        base.PlayerLanded(player);

        //offer to the player if they want to buy this space
        if (owner == null)
        {
            Debug.Log("Do you want to buy this space?");
            Debug.Log("Hell yeah you do!");
            SpaceBought(player);
        }
        else if (owner != player)
        {
            Debug.Log("Pay up!");
            MoneyManager.PlayerTransaction(player, owner, shopValue);
        }
    }

    public override void PlayerPassed(Player player)
    {
        base.PlayerPassed(player);
    }

    public override void SpaceBought(Player player)
    {
        base.SpaceBought(player);
        this.owner = player;
    }
}
