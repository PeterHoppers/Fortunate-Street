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

    const int STOCK_CHANGE_CUTOFF = 10;
    const int STOCK_PRICE_CHANGE_AMOUNT = 1;

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

    /// <summary>
    /// Called whenever a player buys a stock. Also performs the money transaction for the stock. Presumes that the player can afford the stock.
    /// </summary>
    /// <param name="player"></param>
    /// <param name="district"></param>
    /// <param name="amount"></param>
    public static void BuyStock(Player player, District district, int amount) 
    {
        district.stocksBought[player] += amount;
        MoneyManager.MoneyChanged(player, -(district.StockPrice * amount));
        lastPurchase = new StockPurchase(player, district, amount);

        if (amount >= STOCK_CHANGE_CUTOFF)
        {
            ChangeStockValue(district, STOCK_PRICE_CHANGE_AMOUNT);
        }
    }

    /// <summary>
    /// Logic to detemine if a player can attempt to buy the stocks they're trying to buy. Doesn't perform buying the stock even if it can.
    /// Call 'Buy Stock' to confirm the purchase.
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

    /// <summary>
    /// Method that allows the player to see stocks in order to get gold
    /// </summary>
    /// <param name="player"></param>
    /// <param name="district"></param>
    /// <param name="amount"></param>
    public static void SellStock(Player player, District district, int amount)
    {
        district.stocksBought[player] -= amount;
        MoneyManager.MoneyChanged(player, district.StockPrice * amount);

        if (amount >= STOCK_CHANGE_CUTOFF)
        {
            ChangeStockValue(district, -STOCK_PRICE_CHANGE_AMOUNT);
        }
    }

    /// <summary>
    /// When a player goes back past the bank, we need to undo any purchases they might have done
    /// </summary>
    /// <param name="player"></param>
    public static void UndoPurchase(Player player)
    {
        if (lastPurchase != null && lastPurchase.player == player)
        {
            SellStock(lastPurchase.player, lastPurchase.district, lastPurchase.amount);            
        }
    }

    public static int GetStockValue(District district)
    {
        return district.StockPrice;
    }

    /// <summary>
    /// Changes the price of the stock for a district by a fixed number.
    /// </summary>
    /// <param name="district"></param>
    /// <param name="amtChange"></param>
    public static void ChangeStockValue(District district, int amtChange) 
    {
        district.StockPrice += amtChange;
    }

    /// <summary>
    /// Changes the price of a district's stock by a percentage. 100% is 1.
    /// </summary>
    /// <param name="district"></param>
    /// <param name="percentChange"></param>
    public static void ChangeStockValue(District district, double percentChange)
    {
        district.StockPrice += (int) ((percentChange + 1) * district.StockPrice);
    }

    public static int GetPlayerStockInDistrict(Player player, District district)
    {
        return district.stocksBought[player];
    }

    /// <summary>
    /// Calcaulates the total value of a player's stocks and returns that value.
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Whoever owns stock in the district the transaction happened in need to get money for it
    /// </summary>
    /// <param name="space"></param>
    /// <param name="amountSpent"></param>
    public static void DetermineDividends(BuyableSpace space, int amountSpent)
    {
        //TODO: More fancy logic to determine how much money each owner of stock should get, right now all get 10% of the amount spent
        //TODO: What happens when a transaction happens on pass? How do we undo this dividend?
        List<Player> playersOwningStock = new List<Player>();

        foreach (KeyValuePair<Player, int> stocks in space.district.stocksBought)
        {
            if (stocks.Value > 0)
            {
                playersOwningStock.Add(stocks.Key);
            }
        }

        foreach (Player player in playersOwningStock)
        {
            MoneyManager.MoneyChanged(player, (int)(amountSpent * .1));
        }
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
