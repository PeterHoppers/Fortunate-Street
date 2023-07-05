using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// The format of this code pulls from https://docs.unity3d.com/2021.2/Documentation/Manual/UIE-HowTo-CreateRuntimeUI.html#finalCode_mainView
/// </summary>
public class PlayerStatsView : MonoBehaviour
{
    [SerializeField]
    VisualTreeAsset m_PlayerStatsTemplate;

    public void CreatePlayerStats(List<Player> players)
    {
        // The UXML is already instantiated by the UIDocument component
        var uiDocument = GetComponent<UIDocument>();

        // Initialize the character list controller
        var statsListController = new PlayerStatsListController();
        statsListController.InitializePlayersList(uiDocument.rootVisualElement.Query("StatsHolder"), m_PlayerStatsTemplate, players);
    }
}
