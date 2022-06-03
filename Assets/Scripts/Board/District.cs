using System.Collections.Generic;
using UnityEngine;
public class District: MonoBehaviour
{
    public string Name;
    public BuyableSpace[] Spaces;
    public int StockPrice;

    public Dictionary<Player, int> stocksBought = new Dictionary<Player, int>();

    public int PlayerValueForDistrict(Player player)
    {
        return stocksBought[player] * StockPrice;
    }
}
