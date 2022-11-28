using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the logic for anything related to suits
/// </summary>
public class SuitManager
{
    public static Dictionary<Player, List<Suit>> suits = new();
    public const int suitMoneyValue = 100;

    public delegate void PlayerExchangeSuite(Player player, Suit suit, bool isGained);
    public static event PlayerExchangeSuite OnPlayerGotSuit;

    public static void SetupSuits(List<Player> players)
    {
        //foreach Player, add them to the suites dictionary
        foreach (Player player in players)
        {
            if (player != null)
            {
                suits.Add(player, new List<Suit>());
            }
        }

    }

    public static void GetSuit(Player player, Suit suit)
    {
        if (!suits[player].Contains(suit))
        {
            suits[player].Add(suit);
            OnPlayerGotSuit?.Invoke(player, suit, true);
        }
        else 
        {
            //the player gains money according to suitMoneyValue
            MoneyManager.MoneyChanged(player, suitMoneyValue);
        }
    }

    public static void RemoveSuit(Player player, Suit suit)
    {
        if (suits[player].Contains(suit))
        {
            suits[player].Remove(suit);
            OnPlayerGotSuit?.Invoke(player, suit, false);
        }
        else 
        {
            Debug.LogError($"Why did we try to remove suit {suit} from {player.playerName} when they didn't have it?");
        }
        
    }

    public static void ClearSuits(Player player)
    {
        suits[player] = new List<Suit>();
    }
}
