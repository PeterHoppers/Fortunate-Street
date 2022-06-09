using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Property : BuyableSpace
{
    int shopPrice;
    int maxInvestment;

    void Start()
    {
        shopPrice = CalcShopPrice(shopValue);
        maxInvestment = CalcMaxInvestiment(shopValue);
    }

    public void ChangePrice(double percentage)
    {
        //when called, change the shop price, shop value, and investiment amount, if applicatable
    }

    /// <summary>
    /// Do some math to figure out what the price of each store should be based upon their shop value
    /// Do I know the formula that fortune street uses? Not really
    /// </summary>
    /// <param name="shopValue"></param>
    /// <returns></returns>
    public int CalcShopPrice(int shopValue)    
    {
        return (int) (shopValue / 5.25);
    }

    public int GetShopPrice()
    {
        return shopPrice;
    }

    /// <summary>
    /// Do some math to figure out what the price of each store should be based upon their shop value
    /// Do I know the formula that fortune street uses? Yeah, it's just double it
    /// </summary>
    /// <param name="shopValue"></param>
    /// <returns></returns>
    public int CalcMaxInvestiment(int shopValue)
    {
        return shopValue * 2;
    }

    public override void PlayerLanded(Player player)
    {
        base.PlayerLanded(player);

        //offer to the player if they want to buy this space
        if (owner == null)
        {
            Debug.Log("Do you want to buy this space?");
            Debug.Log("Hell yeah you do!");

            //temporary logic to prevent players from buying with money they don't have
            if (MoneyManager.GetPlayerMoneyValue(player) < shopValue)
            {
                Debug.Log("No wait! You're too poor!");
                return;
            }

            MoneyManager.MoneyChanged(player, shopValue * -1);
            SpaceBought(player);
        }
        else if (owner != player)
        {
            Debug.Log("Pay up!");
            MoneyManager.PlayerTransaction(player, owner, shopPrice);
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

        //test way of showing who owns what space
        Renderer spaceRenderer = GetComponent<Renderer>();
        spaceRenderer.material.SetColor("_Color", player.color);
    }
}
