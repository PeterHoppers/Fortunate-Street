using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class StartTurnUI : TurnStateUI
{
    public TMP_Text playerName;
    public override void SetUpTurnState(Player player)
    {
        playerName.text = $"{player.playerName}'s turn";
    }
}
