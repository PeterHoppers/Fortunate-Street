using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerStatsListController
{
    Dictionary<Player, PlayerStatsListEntryController> playersLogic = new Dictionary<Player, PlayerStatsListEntryController>();

    public void InitializePlayersList(VisualElement root, VisualTreeAsset playerStatsTemplate, List<Player> players)
    {
        // Store a reference to the template for the list entries
        //m_ListEntryTemplate = listElementTemplate;
        foreach (Player player in players)
        {
            //we grab the stat holder, rather than the template root so that the top element in the template is what is rendered
            var newListElement = playerStatsTemplate.Instantiate().Query("StatHolder").First();
            root.Add(newListElement);

            // Instantiate a controller for the data
            var playerEntryLogic = new PlayerStatsListEntryController();

            // Initialize the controller script
            playerEntryLogic.SetVisualElement(newListElement);

            playerEntryLogic.SetName(player.playerName);
            playerEntryLogic.SetBackground(player.color);
            playerEntryLogic.SetMoney(MoneyManager.GetPlayerMoneyValue(player));
            playerEntryLogic.SetWorth(GameManager.Instance.GetPlayerTotalWorth(player));

            playersLogic.Add(player, playerEntryLogic);
        }

        MoneyManager.OnPlayerMoneyChanged += UpdatePlayerMoney;
        SuitManager.OnPlayerGotSuit += UpdateSuiteUI;
        GameManager.OnPlayerLevelUp += ClearSuiteUI;
        GameManager.OnPlayerLevelUp += UpdateWorth;
    }

    void UpdatePlayerMoney(Player player, int oldAmount, int newAmount)
    {
        var playerEntryLogic = playersLogic[player];

        Debug.Log($"{player.playerName}'s money has gone from {oldAmount} to {newAmount}");

        playerEntryLogic.SetMoney(newAmount);
    }

    void UpdateSuiteUI(Player player, Suit suit, bool isGained)
    {
        var playerEntryLogic = playersLogic[player];

        playerEntryLogic.SetSuit(suit.ToString(), isGained);        
    }

    void ClearSuiteUI(Player player)
    {
        var playerEntryLogic = playersLogic[player];

        var allSuits = Enum.GetValues(typeof(Suit)).Cast<Suit>().ToList();

        foreach (Suit suit in allSuits)
        {
            playerEntryLogic.SetSuit(suit.ToString(), false);
        }
    }

    void UpdateWorth(Player player)
    {
        var playerEntryLogic = playersLogic[player];
        playerEntryLogic.SetWorth(GameManager.Instance.GetPlayerTotalWorth(player));
    }
}
