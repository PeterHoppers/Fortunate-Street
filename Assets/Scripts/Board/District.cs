using System.Collections.Generic;
using UnityEngine;
public class District: MonoBehaviour
{
    public string Name;
    public Color color; 
    public BuyableSpace[] Spaces;
    public int StockPrice;

    public Dictionary<Player, int> stocksBought = new Dictionary<Player, int>();

    void Start()
    {
        //right now, each space has children that make up the outline for the space
        //this loops through each space, then sets the color for each of space's renderers
        foreach (Space space in Spaces)
        {
            Renderer[] spaceRenderers = space.transform.Find("Outline").GetComponentsInChildren<Renderer>();

            foreach (Renderer renderer in spaceRenderers)
            {
                renderer.material.SetColor("_Color", color);
            }            
        }
    }

    public int PlayerValueForDistrict(Player player)
    {
        return stocksBought[player] * StockPrice;
    }
}
