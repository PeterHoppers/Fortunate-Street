using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class StartTurnUI : TurnStateUI
{
    public Button selectedUI;

    public override void SetupUI()
    {
        gameObject.SetActive(true);
        selectedUI.Select();
        print(EventSystem.current.currentSelectedGameObject);
        string playerName = GameManager.Instance.GetActivePlayerName();
        UIManager.Instance.DisplayMessage($"{playerName}'s turn");
    }

    public override void HideUI()
    {
        gameObject.SetActive(false);
        UIManager.Instance.HideGenericMessage();
    }
}
