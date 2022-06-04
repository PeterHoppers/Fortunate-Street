using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollAgain : Space
{
    public override void PlayerLanded(Player player)
    {
        base.PlayerLanded(player);
        //roll again
    }

    public override void PlayerPassed(Player player)
    {
        base.PlayerPassed(player);
        //do nothing
    }
}
