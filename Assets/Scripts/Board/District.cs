using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class District: MonoBehaviour
{
    public string Name;
    public Color color; 
    public BuyableSpace[] spaces;
    public int StockPrice;

    public Dictionary<Player, int> stocksBought = new Dictionary<Player, int>();

    void OnEnable()
    {
        foreach (BuyableSpace space in spaces) 
        {
            space.OnSpaceBought += DistrictShopBought;
            space.district = this;
        }
    }

    void OnDisable()
    {
        foreach (BuyableSpace space in spaces)
        {
            space.OnSpaceBought -= DistrictShopBought;
        }
    }

    void Start()
    {
        //right now, each space has children that make up the outline for the space
        //this loops through each space, then sets the color for each of space's renderers
        foreach (Space space in spaces)
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

    /// <summary>
    /// When a shop is bought in a district, check to see if there are any other ones in that district. Increase the value of those shops accordingly
    /// </summary>
    /// <param name="player"></param>
    /// <param name="space"></param>
    void DistrictShopBought(Player player, BuyableSpace space)
    {
        Debug.Log($"{Name} is checking if the player has already bought a shop in this district.");
        BuyableSpace[] spacesPlayerOwns = spaces.Where(shop => shop.owner == player).ToArray();
        Debug.Log($"{player.playerName} owns {spacesPlayerOwns.Length} shops in {Name} district.");

        if (spacesPlayerOwns.Length > 1) {
            foreach (BuyableSpace ownedSpace in spacesPlayerOwns) 
            {
                ownedSpace.ChangeShopValue(spacesPlayerOwns.Length);
            }
        }

    }
}
