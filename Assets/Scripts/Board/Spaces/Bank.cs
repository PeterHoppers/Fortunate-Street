using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bank : Space
{
    GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.GetGameManager();
    }
    public override void PlayerPassed(Player player)
    {
        base.PlayerPassed(player);
        BankVisit(player);
        gameManager.CheckPlayerWon(player);
    }

    public override void PlayerLanded(Player player)
    {
        base.PlayerLanded(player);
        BankVisit(player);
        gameManager.CheckPlayerWon(player);
        //clear movement choice for the player
    }

    //TODO: Move Level Up logic to the game mananger
    void BankVisit(Player player)
    {
        int suiteCount = SuiteManager.suites[player].Count;
        Debug.Log($"Sutie count is: {suiteCount}");
        //we also need to check for suite yourself cards

        if (suiteCount >= 4)
        {
            gameManager.LevelUpPlayer(player);
            SuiteManager.ClearSuites(player);
            Debug.Log("Leveled up!");
        }

        //prompt the player to buy stocks
    }
}
