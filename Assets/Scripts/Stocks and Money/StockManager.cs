using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Controls the logic for anything related to stocks.
/// </summary>
public class StockManager
{
    static District[] districts;
    static StockPurchase lastPurchase; 

    public static void SetupStocks(List<Player> players)
    {
        districts = BoardManager.districts.ToArray();

        //initalize 0 stocks for each player
        foreach (District district in districts)
        {
            foreach (Player player in players)
            {
                district.stocksBought[player] = 0;
            }
        }        
    }

    public static void BuyStock(Player player, District district, int amount) 
    {
        district.stocksBought[player] += amount;
        MoneyManager.MoneyChanged(player, -district.StockPrice * amount);
        Debug.Log($"{player} bought {amount} of {district}'s stock.");
        lastPurchase = new StockPurchase(player, district, amount);
    }

    /// <summary>
    /// Logic to detemine if a player can attempt to buy the stocks they're trying to buy
    /// </summary>
    /// <param name="player"></param>
    /// <param name="district"></param>
    /// <param name="amount"></param>
    /// <returns></returns>
    public static bool CanBuyStock(Player player, District district, int amount)
    {
        int playerFunds = MoneyManager.GetPlayerMoneyValue(player);

        return district.StockPrice * amount <= playerFunds;        
    }

    public static void SellStock(Player player, District district, int amount)
    {
        district.stocksBought[player] -= amount;
    }

    /// <summary>
    /// When a player goes back past the bank, we need to undo any purchases they might have done
    /// </summary>
    /// <param name="player"></param>
    public static void UndoPurchase(Player player)
    {
        if (lastPurchase.player == player)
        {
            SellStock(lastPurchase.player, lastPurchase.district, lastPurchase.amount);
            MoneyManager.MoneyChanged(player, lastPurchase.district.StockPrice * lastPurchase.amount);
        }
    }

    public static int GetStockValue(District district)
    {
        return district.StockPrice;
    }

    public static void ChangeStockValue(District district, int amtChange) 
    {
        district.StockPrice += amtChange;
    }

    public static void ChangeStockValue(District district, double percentChange)
    {
        //percent change to the stockPrice
        district.StockPrice += (int) ((percentChange + 1) * district.StockPrice);
    }

    public static int GetPlayerStockInDistrict(Player player, District district)
    {
        return district.stocksBought[player];
    }


    public static int GetPlayerStockWorth(Player player)
    {
        int stockWorth = 0;

        foreach (District district in districts)
        {
            if (district.stocksBought.ContainsKey(player))
            {
                stockWorth += district.stocksBought[player] * district.StockPrice;
            }
        }

        return stockWorth;
    }
}

public class StockPurchase
{
    public Player player;
    public District district;
    public int amount;

    public StockPurchase(Player player, District district, int amount)
    {
        this.player = player;
        this.district = district;
        this.amount = amount;
    }
}
