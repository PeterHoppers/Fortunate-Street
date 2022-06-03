using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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

    public static void MovePlayerToSpace(Player player, Space space) 
    {
        player.GetMoved(space);
    }

    public static List<BuyableSpace> GetPlayerShops(Player player)
    {
        return shops.Where(s => s.owner == player).ToList();
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
}
