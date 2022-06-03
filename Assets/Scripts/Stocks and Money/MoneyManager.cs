using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Keeps track of all the logic needed to calucate money and monitary transactions
/// </summary>
public class MoneyManager
{
    public static Dictionary<Player, int> wallet = new Dictionary<Player, int>();

    // Start is called before the first frame update

    public static void SetupMoney(Player[] players, int startingMoney)
    {
        foreach (Player player in players)
        {
            if (player != null)
            {
                wallet.Add(player, startingMoney);
            }
        }
    }

    public static void GainMoney(Player player, int amount)
    {
        wallet[player] += amount;
    }

    public static void PlayerTransaction(Player payer, Player getter, int amount)
    {
        wallet[payer] -= amount;
        wallet[getter] += amount;
    }

    public static int GetPlayerMoneyValue(Player player)
    {
        return wallet[player];
    }
}
