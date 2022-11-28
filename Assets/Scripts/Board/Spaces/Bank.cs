using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bank : Space
{
    GameManager gameManagerforBank;

    void Start()
    {
        gameManagerforBank = Camera.main.GetComponent<GameManager>();
    }
    public override void PlayerPassed(Player player)
    {
        base.PlayerPassed(player);
        //we will need to do something to wait for the level up animation to end before checking
        int suiteCount = SuitManager.suits[player].Count;

        if (suiteCount >= 4)
        {
            gameManagerforBank.LevelUpPlayer(player);
        }

        if (!gameManagerforBank.CheckPlayerWon(player)) 
        {
            UIManager.Instance.ToggleStockPurchaseDisplay(player);
        }
    }

    public override void PlayerLanded(Player player)
    {
        base.PlayerLanded(player);
        player.SetTurnState(TurnState.Finished);
        //clear movement choice for the player
    }

    public override void PlayerReversed(Player player)
    {
        base.PlayerLanded(player);
        StockManager.UndoPurchase(player);
    }
}
