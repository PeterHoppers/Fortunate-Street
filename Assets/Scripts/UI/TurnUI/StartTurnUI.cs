using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class StartTurnUI : TurnStateUI
{
    public override void SetupUI(Player player)
    {
        gameObject.SetActive(true);
        UIManager.Instance.DisplayMessage($"{player.playerName}'s turn");
    }

    public override void HideUI()
    {
        gameObject.SetActive(false);
        UIManager.Instance.HideGenericMessage();
    }
}
