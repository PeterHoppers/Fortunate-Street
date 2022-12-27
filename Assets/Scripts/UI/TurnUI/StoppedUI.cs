using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoppedUI : TurnStateUI
{
    public Button selectedUI;
    public override void SetupUI()
    {
        gameObject.SetActive(true);
        selectedUI.Select();
    }

    public override void HideUI()
    {
        gameObject.SetActive(false);
    }
}
