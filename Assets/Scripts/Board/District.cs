using System.Collections.Generic;
public class District
{
    public string Name { get; set; }
    public BuyableSpace[] Spaces { get; set; }
    public int StockPrice { get; set; }
    public Dictionary<Player, int> stocksBought = new Dictionary<Player, int>();
}
