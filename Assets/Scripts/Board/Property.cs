using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Property : BuyableSpace
{
    public double shopPrice;
    public double investmentAmt;
    public double shopValue;

    public void ChangePrice(double percentage)
    {
        //when called, change the shop price, shop value, and investiment amount, if applicatable
    }

    public override void OnPlayerLand(Player player)
    {
        throw new System.NotImplementedException();
    }

    public override void OnPlayerPass(Player player)
    {
        throw new System.NotImplementedException();
    }

    public override void OnSpaceBought(Player player)
    {
        throw new System.NotImplementedException();
    }
}
