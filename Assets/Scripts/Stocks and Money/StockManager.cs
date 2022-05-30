using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class StockManager : MonoBehaviour
{
    District[] districts;
    // Start is called before the first frame update
    void Start()
    {
        districts = BoardManager.districts.ToArray();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BuyStock(Player player, District district, int amount) 
    {
        district.stocksBought[player] += amount;
    }

    public void ChangeStockValue(District district, int amtChange) 
    {
        district.StockPrice += amtChange;
    }

    public void ChangeStockValue(District district, double percentChange)
    {
        //percent change to the stockPrice
        district.StockPrice += (int) ((percentChange + 1) * district.StockPrice);
    }

    public double GetPlayerStockWorth(Player player)
    {
        double stockWorth = 0;

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
