using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MovingUI : TurnStateUI
{
    public TMP_Text spacesLeftText;

    // Start is called before the first frame update
    void OnEnable()
    {
        Space.OnPlayerPass += SpacesToMoveUpdate;
        Space.OnPlayerReverse += SpacesToMoveUpdate;
    }

    void OnDisable()
    {
        Space.OnPlayerPass -= SpacesToMoveUpdate;
        Space.OnPlayerReverse -= SpacesToMoveUpdate;
    }

    public override void SetUpTurnState(Player player)
    {
        UpdateSpaceDisplay(player.GetSpacesLeftToMove());
    }

    public void SpacesToMoveUpdate(Player player, Space space)
    {
        UpdateSpaceDisplay(player.GetSpacesLeftToMove());
    }

    void UpdateSpaceDisplay(int spaceAmount)
    {
        spacesLeftText.text = spaceAmount.ToString();
    }
}
