using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class StartTurnUI : TurnStateUI
{
    public override void SetUpTurnState(Player player)
    {
        UIManager.DisplayMessage($"{player.playerName}'s turn");
    }
}
