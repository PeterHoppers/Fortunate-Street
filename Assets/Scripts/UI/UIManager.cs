using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    /// <summary>
    /// Setting up an Singleton instance for the UIManager to allow spaces and other UI to request what should appear
    /// </summary>
    public static UIManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    public PlayerStatsView allPlayerStatsPanel;
    public GameObject playerMoneyStatsPanel;
    public GameObject statsPanel;

    public GenericPromptUI genericPromptUI;
    public PropertyPurchaseUI propertyBuyUI;
    public StockPurchaseUI stockPurchaseUI;

    public TurnStateUI[] uiStates;

    Player playerTurn;
    TurnState currentTurnState;

    /// <summary>
    /// Function for setting up any UI that gets set up once at the start of the game
    /// </summary>
    /// <param name="players"></param>
    public void SetupUI(List<Player> players)
    {
        allPlayerStatsPanel.CreatePlayerStats(players);
        /*
        foreach (Player player in players)
        {
            if (player != null)
            {
                GameObject statPanel = Instantiate(statsPanel, allPlayerStatsPanel.transform);
                PlayerOverviewUI overview = statPanel.GetComponent<PlayerOverviewUI>();

                if (overview == null)
                {
                    Debug.LogError("Game object provided for player overview does not have the right script attached");
                    return;
                }

                overview.SetPlayer(player);
            }
        }*/

        // TODO: If we actually change the visibility of the all player stats panel, it can't update the money value when it is hidden
        //allPlayerStatsPanel.SetActive(true);
        playerMoneyStatsPanel.SetActive(false);
        HidePropertyPurchaseDisplay();
        HideStockPurchaseDisplay();

        foreach (TurnStateUI turnUIState in uiStates)
        {
            turnUIState.HideUI();
        }
    }

    void OnEnable()
    {
        GameManager.OnPlayerActiveChange += PlayerChanged;
    }

    void OnDisable()
    {
        GameManager.OnPlayerActiveChange -= PlayerChanged;
    }

    /// <summary>
    /// When the active player has changed, update the turn ui to match the information found on that player
    /// </summary>
    /// <param name="player"></param>
    void PlayerChanged(Player player)
    {
        //remove the reference to previous player first
        if (playerTurn)
        {
            playerTurn.OnPlayerTurnStateChanged -= DisplayStateUI;
        }

        playerTurn = player;
        playerTurn.OnPlayerTurnStateChanged += DisplayStateUI;

        //reset any UI that needs to be someway at the start of the turn
        //allPlayerStatsPanel.SetActive(true);
        playerMoneyStatsPanel.SetActive(false);
    }

    /// <summary>
    /// These methods here are calling methods found of the player so that the UI can control the player
    /// </summary>

    public void ToggleMoneyDisplay(Player player)
    {
        //allPlayerStatsPanel.SetActive(false); 
        playerMoneyStatsPanel.SetActive(true);
        playerMoneyStatsPanel.GetComponent<PlayerMoneyOverviewUI>().DisplayPlayer(player);
    }

    /// <summary>
    /// Use to display a message to the UI
    /// </summary>
    /// <param name="messageToDisplay"></param>
    public void DisplayMessage(string messageToDisplay, UnityAction calledFunction)
    {
        genericPromptUI.DisplayPrompt(messageToDisplay, calledFunction);
    }

    public void DisplayMessage(string messageToDisplay)
    {
        genericPromptUI.DisplayPrompt(messageToDisplay);
    }

    /// <summary>
    /// Hide the generic message that appears in 'Display Message'
    /// </summary>
    public void HideGenericMessage()
    {
        genericPromptUI.ClosePrompt();
    }

    public void TogglePropertyPurchaseDisplay(Player player, Property property)
    {
        propertyBuyUI.SetupUI(player, property);
    }

    /// <summary>
    /// Normally, the player will simply close out the UI with clicking a button, however, we need the AI to be able to close out the menus as well
    /// </summary>
    public void HidePropertyPurchaseDisplay()
    {
        propertyBuyUI.HideUI();
    }

    public void ToggleStockPurchaseDisplay(Player player)
    {
        stockPurchaseUI.SetupUI(player);
    }

    public void HideStockPurchaseDisplay()
    {
        stockPurchaseUI.gameObject.SetActive(false);
    }

    /// <summary>
    /// Update the UI to display the content in said state
    /// </summary>
    /// <param name="turnState"></param>
    void DisplayStateUI(TurnState turnState)
    {
        TurnStateUI previousState = uiStates.Where(ui => ui.state == currentTurnState).FirstOrDefault();
        previousState?.HideUI();
        TurnStateUI nextState = uiStates.Where(ui => ui.state == turnState).FirstOrDefault();
        nextState?.SetupUI();
       
        currentTurnState = turnState;
    }
}