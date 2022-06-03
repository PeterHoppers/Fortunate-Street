using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuiteSpace : Space
{
    public Suite suite;
    public override void OnPlayerLand(Player player)
    {
        SuiteManager.GetSuite(player, suite);
        //run chance time
    }

    public override void OnPlayerPass(Player player)
    {
        SuiteManager.GetSuite(player, suite);
    }
}
