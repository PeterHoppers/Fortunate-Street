using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the logic for anything related to suites (or suit, or whatever we rename this to)
/// </summary>
public class SuiteManager
{
    public static Dictionary<Player, List<Suite>> suites = new Dictionary<Player, List<Suite>>();
    public const int suitMoneyValue = 100;

    public delegate void PlayerExchangeSuite(Player player, Suite suite, bool isGained);
    public static event PlayerExchangeSuite OnPlayerGotSuite;

    public static void SetupSuites(List<Player> players)
    {
        //foreach Player, add them to the suites dictionary
        foreach (Player player in players)
        {
            if (player != null)
            {
                suites.Add(player, new List<Suite>());
            }
        }

    }

    public static void GetSuite(Player player, Suite suite)
    {
        if (!suites[player].Contains(suite))
        {
            suites[player].Add(suite);
            Debug.Log($"{player.name} got {suite}");
            OnPlayerGotSuite?.Invoke(player, suite, true);
        }
        else 
        {
            //the player gains money according to suitMoneyValue
            MoneyManager.MoneyChanged(player, suitMoneyValue);
        }
    }

    public static void RemoveSuite(Player player, Suite suite)
    {
        if (suites[player].Contains(suite))
        {
            suites[player].Remove(suite);
            OnPlayerGotSuite?.Invoke(player, suite, false);
        }
        else 
        {
            Debug.LogError($"Why did we try to remove suite {suite} from {player.playerName} when they didn't have it?");
        }
        
    }

    public static void ClearSuites(Player player)
    {
        suites[player] = new List<Suite>();
    }
}
