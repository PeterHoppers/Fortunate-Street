using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class StartTurnUI : TurnStateUI
{
    public override void SetupUI()
    {
        gameObject.SetActive(true);
        string playerName = GameManager.Instance.GetActivePlayerName();
        UIManager.Instance.DisplayMessage($"{playerName}'s turn");
    }

    public override void HideUI()
    {
        gameObject.SetActive(false);
        UIManager.Instance.HideGenericMessage();
    }
}
