using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// The UI for the landed Turn State. This is the most complicated one, since it depends on which space you landed on.
/// Therefore, the SetupTurnState logic goes through all possible landed space types and their landed UIs
/// </summary>
public class LandedUI : TurnStateUI
{
    public LandedUIToSpace[] landedUIToSpaces;

    /// <summary>
    /// Displays different UI depending on the type of space that you landed on
    /// </summary>
    /// <param name="player"></param>
    public override void SetUpTurnState(Player player)
    {
        foreach (LandedUIToSpace uis in landedUIToSpaces)
        {
            uis.spaceLandedUI.gameObject.SetActive(false);
        }

        Space landedSpace = player.GetCurrentSpace();

        string spaceType = landedSpace.GetType().Name;

        LandedUIToSpace uiToDisplay = landedUIToSpaces.Where(x => x.spaceType == spaceType).FirstOrDefault();

        if (uiToDisplay != null)
        {
            uiToDisplay.spaceLandedUI.gameObject.SetActive(true);
            uiToDisplay.spaceLandedUI.SetUpUI(player, landedSpace);
        }
        else 
        {
            Debug.LogWarning($"Could not find a UI associated with the space type: {spaceType}.");

            //right now, there's a bug with the Roll Again space not working without the UI for it, since the turn doesn't finish when they land on it
            if (spaceType != "RollAgain")
            {
                player.SetTurnState(TurnState.Finished);
            }
        }
    }
}

/// <summary>
/// A class that allows a UI to be matched up with a matching string for whatever the space is called
/// </summary>
[System.Serializable]
public class LandedUIToSpace
{
    public LandedUIBaseSpace spaceLandedUI;
    public string spaceType;
}

public abstract class LandedUIBaseSpace : MonoBehaviour
{
    public abstract void SetUpUI(Player player, Space space);
    public abstract void TurnOffUI();
}
