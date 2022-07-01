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
        gameManagerforBank.CheckPlayerLevelUp(player);
        gameManagerforBank.CheckPlayerWon(player);
    }

    public override void PlayerLanded(Player player)
    {
        base.PlayerLanded(player);
        //clear movement choice for the player
    }
}
