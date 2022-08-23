using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Property : BuyableSpace
{
    int originalValue;
    int shopPrice;
    int maxInvestment;

    void Start()
    {
        originalValue = shopValue;
        shopPrice = CalcShopPrice(shopValue);
        maxInvestment = CalcMaxInvestiment(shopValue);
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
        //this prompts the UI for landing on a space
        base.PlayerLanded(player);

        //TODO: Find a way so that the manager doesn't need to be brought in here. Or, find a better way to toggle this panel
        UIManager.Instance.ToggleMoneyDisplay(player);

        //if the space has not been bought, give the player the prompt to buy it
        //else, if the player owns the space, give them the option to invest in it
        //if neither true, skip for now
        if (owner == null)
        {
            UIManager.Instance.TogglePropertyPurchaseDisplay(player, this);
        }
        else if (owner == player)
        {
            //ActiveUI(investUI);
        }
        else //TODO: buyout logic
        {
            MoneyManager.PlayerTransaction(player, owner, shopPrice);
            player.SetTurnState(TurnState.Finished);
        }
    }

    public bool CheckOwner(Player possibleOwner)
    {
        return (owner == possibleOwner);
    }

    public bool CanBuyProperty(Player possibleBuyer)
    {
        return (MoneyManager.GetPlayerMoneyValue(possibleBuyer) >= shopValue) && (owner == null);
    }

    public void BuySpace(Player buyer)
    {
        //temporary logic to prevent players from buying with money they don't have
        if (!CanBuyProperty(buyer))
        {
            Debug.Log("No wait! You're too poor!");
            return;
        }

        MoneyManager.MoneyChanged(buyer, shopValue * -1);
        SpaceBought(buyer);
        buyer.SetTurnState(TurnState.Finished);
    }

    public void DeclineBuySpace(Player player)
    {
        player.SetTurnState(TurnState.Finished);
    }

    public override void PlayerPassed(Player player)
    {
        base.PlayerPassed(player);
    }

    public override void SpaceBought(Player player)
    {
        base.SpaceBought(player);        

        //test way of showing who owns what space
        Renderer spaceRenderer = GetComponent<Renderer>();
        spaceRenderer.material.SetColor("_Color", player.color);
    }

    /// <summary>
    /// Updates the value of the shop and adjusts the max investiment and shop price accordingly
    /// </summary>
    /// <param name="amountChange"></param>
    public override void ChangeShopValue(double amountChange)
    {
        shopValue = (int)(originalValue * amountChange);
        shopPrice = CalcShopPrice(shopValue);
        maxInvestment = CalcMaxInvestiment(shopValue);
        ValueChanged();
    }
}
