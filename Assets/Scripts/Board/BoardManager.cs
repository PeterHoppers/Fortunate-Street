using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BoardManager : MonoBehaviour
{
    public static List<Space> board = new List<Space>();
    public static List<District> districts = new List<District>();
    static List<BuyableSpace> shops = new List<BuyableSpace>();

    void Start()
    {
        //foreach space in board, if it is a buyable space, add it to shops
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
    }
}
