using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollAgain : Space
{
    public override void PlayerLanded(Player player)
    {
        //roll again
        player.StartRolling();
    }

    public override void PlayerPassed(Player player)
    {
        base.PlayerPassed(player);
        //do nothing
    }
}
