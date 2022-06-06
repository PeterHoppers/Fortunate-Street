using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bank : Space
{
    GameManager gameManager;

    void Start()
    {
        gameManager = Camera.main.GetComponent<GameManager>();
    }
    public override void PlayerPassed(Player player)
    {
        base.PlayerPassed(player);
        gameManager.CheckPlayerLevelUp(player);
        gameManager.CheckPlayerWon(player);
    }

    public override void PlayerLanded(Player player)
    {
        base.PlayerLanded(player);
        gameManager.CheckPlayerLevelUp(player);
        gameManager.CheckPlayerWon(player);
        //clear movement choice for the player
    }
}
