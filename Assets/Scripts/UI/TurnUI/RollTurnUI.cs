using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollTurnUI : TurnStateUI
{    public override void SetUpTurnState(Player player)
    {
        UIManager.DisplayMessage($"Press Space to Confirm Roll!");
    }
}
