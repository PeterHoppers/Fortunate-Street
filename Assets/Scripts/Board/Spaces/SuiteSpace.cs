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
        SuiteManager.GetSuite(player, suite);
        //run chance time
    }

    public override void PlayerReversed(Player player)
    {
        base.PlayerReversed(player);
        SuiteManager.RemoveSuite(player, suite);
    }

}
