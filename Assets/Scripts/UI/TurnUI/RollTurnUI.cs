using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI that appears when the player is rolling the dice
/// </summary>
public class RollTurnUI : TurnStateUI
{
    public override void SetupUI()
    {
        UIManager.Instance.DisplayMessage($"Press Space to Roll the Dice!");
    }

    public override void HideUI()
    {
        UIManager.Instance.HideGenericMessage();
    }

}
