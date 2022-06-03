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

    public static void SetupStocks()
    {
        districts = BoardManager.districts.ToArray();
    }

    public static void BuyStock(Player player, District district, int amount) 
    {
        district.stocksBought[player] += amount;
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
