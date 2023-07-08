using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Keeps track of the logic needed to understand how the board is connected and who owns pieces of the board
/// </summary>
public class BoardManager : MonoBehaviour
{
    public static Space[] board;
    static BuyableSpace[] shops;
    public static District[] districts;
    static BoardSection[] sections;

    public static void SetupBoard()
    {
        board = FindObjectsOfType<Space>();
        shops = FindObjectsOfType<BuyableSpace>();
        sections = FindObjectsOfType<BoardSection>();
        districts = FindObjectsOfType<District>();
    }

    /// <summary>
    /// Moves the player to the specificied spot. HasLanded allows the player to trigger the landed condition for the space or not
    /// </summary>
    /// <param name="player"></param>
    /// <param name="space"></param>
    /// <param name="hasLanded"></param>
    public static void MovePlayerToSpace(Player player, Space space, bool hasLanded) 
    {
        player.GetMoved(space, hasLanded);
    }

    public static bool CanBuySpace(Player possibleBuyer, BuyableSpace buyableSpace)
    {
        return (buyableSpace.owner == null && MoneyManager.GetPlayerMoneyValue(possibleBuyer) >= buyableSpace.shopValue);
    }

    public static void BuySpace(Player buyer, BuyableSpace buyableSpace)
    {
        int moneySpent = buyableSpace.shopValue;
        buyableSpace.SpaceBought(buyer);
        MoneyManager.MoneyChanged(buyer, moneySpent * -1);
        GameManager.Instance.AdvanceTurn();
    }

    public static List<BuyableSpace> GetPlayerShops(Player player)
    {
        return shops.Where(shop => 
               shop.owner == player)
               .ToList();
    }

    public static void ClosePlayerShops(Player player)
    {
        foreach (BuyableSpace space in GetPlayerShops(player))
        {
            space.isOpen = false;
        }

        //note, at the start of a player's turn, their shops reopen
        //need logic to keep track of that
    }

    //return the boardsection that is holding the target space
    public static BoardSection GetBoardSectionOfSpace(Space space)
    {
        return sections.Where(section => section.spaces.Contains(space)).First();
    }

    /// <summary>
    /// Convert the properties that player owns into a number of its total value
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public static int GetPlayerBoardValue(Player player)
    {
        List<BuyableSpace> ownedProperties = GetPlayerShops(player);
        int totalValue = ownedProperties.Sum(p => p.shopValue);

        return totalValue;
    }
}
