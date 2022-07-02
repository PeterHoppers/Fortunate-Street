using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LandedUI : TurnStateUI
{
    public LandedUIToSpace[] landedUIToSpaces;

    public override void SetUpTurnState(Player player)
    {
        foreach (LandedUIToSpace uis in landedUIToSpaces)
        {
            uis.uiParent.SetActive(false);
        }

        Space landedSpace = player.GetCurrentSpace();

        string spaceType = landedSpace.GetType().Name;

        LandedUIToSpace uiToDisplay = landedUIToSpaces.Where(x => x.spaceType == spaceType).FirstOrDefault();

        if (uiToDisplay != null)
        {
            uiToDisplay.uiParent.SetActive(true);
            LandedUIBaseSpace landedUIBase = uiToDisplay.uiParent.GetComponent<LandedUIBaseSpace>();

            if (landedUIBase != null)
            {
                landedUIBase.SetUpBaseSpace(player, landedSpace);
            }
        }
        else 
        {
            Debug.LogWarning($"Could not find a UI associated with the space type: {spaceType}.");
            player.SetTurnState(TurnState.Finished);
        }
    }
}

[System.Serializable]
public class LandedUIToSpace
{
    public GameObject uiParent;
    public string spaceType;
}

public abstract class LandedUIBaseSpace : MonoBehaviour
{
    public abstract void SetUpBaseSpace(Player player, Space space);
}
