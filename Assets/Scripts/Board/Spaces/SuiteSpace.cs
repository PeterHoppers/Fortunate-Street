using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuiteSpace : Space
{
    public Suite suite;
    public override void PlayerPassed(Player player)
    {
        base.PlayerPassed(player);
        SuiteManager.GetSuite(player, suite);
    }

    public override void PlayerLanded(Player player)
    {
        base.PlayerLanded(player);
        //run chance time. For now, just end the turn
        player.SetTurnState(TurnState.Finished);
    }

    public override void PlayerReversed(Player player)
    {
        base.PlayerReversed(player);
        SuiteManager.RemoveSuite(player, suite);
    }

}
