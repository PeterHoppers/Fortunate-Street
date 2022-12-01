using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MovingUI : TurnStateUI
{
    public TMP_Text spacesLeftText;
    int spacesLeft;

    // Start is called before the first frame update
    void OnEnable()
    {
        Space.OnPlayerPass += MovedForward;
        Space.OnPlayerReverse += MovedBackward;
    }

    void OnDisable()
    {
        Space.OnPlayerPass -= MovedForward;
        Space.OnPlayerReverse -= MovedBackward;
    }

    public override void SetupUI()
    {
        spacesLeftText.transform.parent.gameObject.SetActive(true);
        spacesLeft = GameManager.Instance.GetSpacesRemaining();
        UpdateSpaceDisplay(spacesLeft);
    }
    public override void HideUI()
    {
        spacesLeftText.transform.parent.gameObject.SetActive(false);
    }

    void MovedForward(Player player, Space space)
    {
        spacesLeft--;
        UpdateSpaceDisplay(spacesLeft);
    }

    void MovedBackward(Player player, Space space)
    {
        spacesLeft++;
        UpdateSpaceDisplay(spacesLeft);
    }

    void UpdateSpaceDisplay(int spaceAmount)
    {
        spacesLeftText.text = spaceAmount.ToString();
    }

    
}
