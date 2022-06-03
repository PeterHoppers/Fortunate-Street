using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bank : Space
{
    public override void OnPlayerPass(Player player)
    {
        BankVisit(player);
    }

    public override void OnPlayerLand(Player player)
    {
        BankVisit(player);
        //clear movement choice for the player
    }

    void BankVisit(Player player)
    {
        int suiteCount = SuiteManager.suites[player].Count;
        //we also need to check for suite yourself cards

        if (suiteCount >= 4)
        {
            GameManager.GetGameManager().LevelUpPlayer(player);
            SuiteManager.ClearSuites(player);
        }

        //prompt the player to buy stocks
    }
}
