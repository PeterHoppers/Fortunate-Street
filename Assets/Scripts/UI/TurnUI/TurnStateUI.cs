using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An abstract class used by each different type of TurnState to create a set of UI that appears when that part of the turn arrises
/// For example, RollUI implements TurnStateUI as is then configured in UIManager to show up when the Roll TurnState occurs
/// </summary>
public abstract class TurnStateUI : MonoBehaviour
{
    public TurnState state;
    public abstract void SetupUI();
    public abstract void HideUI();
}
