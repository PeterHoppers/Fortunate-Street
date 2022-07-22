using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollAgainUI : LandedUIBaseSpace
{
    public override void SetUpUI(Player player, Space space)
    {
        UIManager.DisplayMessage("Roll again!");
    }

    public override void TurnOffUI()
    {
        UIManager.HideGenericMessage();
    }
}
