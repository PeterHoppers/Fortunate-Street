using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuitSpace : Space
{
    public Suit suit;
    public override void PlayerPassed(Player player)
    {
        base.PlayerPassed(player);
        SuitManager.GetSuit(player, suit);
    }

    public override void PlayerLanded(Player player)
    {
        base.PlayerLanded(player);
        //run chance time. For now, just end the turn
        GameManager.Instance.AdvanceTurn();
    }

    public override void PlayerReversed(Player player)
    {
        base.PlayerReversed(player);
        SuitManager.RemoveSuit(player, suit);
    }

}
