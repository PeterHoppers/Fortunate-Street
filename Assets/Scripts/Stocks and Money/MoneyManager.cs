using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Keeps track of all the logic needed to calucate money and monitary transactions
/// </summary>
public class MoneyManager
{
    public static Dictionary<Player, int> wallet = new Dictionary<Player, int>();

    public delegate void PlayerMoneyChanged(Player player, int oldAmount, int newAmount);
    public static event PlayerMoneyChanged OnPlayerMoneyChanged;

    // Start is called before the first frame update

    public static void SetupMoney(List<Player> players, int startingMoney)
    {
        foreach (Player player in players)
        {
            if (player != null)
            {
                wallet.Add(player, startingMoney);
            }
        }
    }
    /// <summary>
    /// The change in money for a user. Enter amount as a negative for if they spend the money
    /// </summary>
    /// <param name="player"></param>
    /// <param name="amount"></param>
    public static void MoneyChanged(Player player, int amount)
    {
        int previousAmt = wallet[player];
        wallet[player] += amount;
        OnPlayerMoneyChanged?.Invoke(player, previousAmt, wallet[player]);
    }

    public static void PlayerTransaction(Player payer, Player getter, int amount)
    {
        MoneyChanged(payer, amount * -1);
        MoneyChanged(getter, amount);
    }

    public static int GetPlayerMoneyValue(Player player)
    {
        return wallet[player];
    }
}
