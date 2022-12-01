using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoppedUI : TurnStateUI
{
    public override void SetupUI()
    {
        gameObject.SetActive(true);
    }

    public override void HideUI()
    {
        gameObject.SetActive(false);
    }
}
