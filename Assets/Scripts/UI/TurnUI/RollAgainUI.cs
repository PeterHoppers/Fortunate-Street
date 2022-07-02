using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollAgainUI : LandedUIBaseSpace
{
    Player player;
    public override void SetUpBaseSpace(Player player, Space space)
    {
        this.player = player;
    }
}
