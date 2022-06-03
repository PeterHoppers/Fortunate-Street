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

    public override void OnPlayerLand(Player player)
    {
        //offer to the player if they want to buy this space
        if (owner == null)
        {
            Debug.Log("Do you want to buy this space?");
            Debug.Log("Hey, yay, you do!");
            OnSpaceBought(player);
        }
        else if (owner != player)
        {
            Debug.Log("Pay up!");
            MoneyManager.PlayerTransaction(player, owner, shopValue);
        }
        
    }

    public override void OnPlayerPass(Player player)
    {
        
    }

    public override void OnSpaceBought(Player player)
    {
        this.owner = player;
    }
}
