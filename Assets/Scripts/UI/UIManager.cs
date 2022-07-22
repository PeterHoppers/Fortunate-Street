using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameObject allPlayerStatsPanel;
    public GameObject statsPanel;

    public GameObject propertyBuyUI;
    public TurnUIState[] uiStates;

    //this is done as static so that other UI elements can call it without having the worrying about what the UI Manager is or where it is
    public static TMP_Text genericPromptUI;

    Player playerTurn;

    /// <summary>
    /// Function for setting up any UI that gets set up once at the start of the game
    /// </summary>
    /// <param name="players"></param>
    public void SetupUI(List<Player> players)
    {        
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
        }
    }

    void Awake()
    {
        genericPromptUI = GameObject.FindGameObjectWithTag("GenericPrompt").GetComponent<TMP_Text>(); //can't add a reference through the UI for static objects
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
        DisplayStateUI(TurnState.BeforeRoll);
    }

    /// <summary>
    /// These methods here are calling methods found of the player so that the UI can control the player
    /// </summary>
    public void RollDice()
    {
        playerTurn.StartRolling();
    }

    public void ContinueMoving()
    {
        playerTurn.ContinueMoving();
    }

    public void EndMoving()
    {
        playerTurn.EndMoving();
    }

    /// <summary>
    /// Update the UI to display the content in said state
    /// </summary>
    /// <param name="turnState"></param>
    void DisplayStateUI(TurnState turnState)
    {
        //right now, there's a bug with finished where it gets called after the next player's before roll is called
        if (turnState == TurnState.Finished)
        {
            return;
        }
        //deactivate all other state UI
        foreach (TurnUIState turnUIState in uiStates)
        {
            turnUIState.uiStateHolder.SetActive(false);
            HideGenericMessage();
        }

        TurnUIState stateUIHolder = uiStates.Where(ui => ui.state == turnState).FirstOrDefault();

        if (stateUIHolder == null)
        {
            return;
        }

        //set the active state ui to be true
        stateUIHolder.uiStateHolder.SetActive(true);

        //if the state is complicated enough, there is a base class called 'TurnStateUI' that can be created for each turn state
        //this then takes in the player and updates its UI with the info found in the player
        TurnStateUI stateUILogic = stateUIHolder.uiStateHolder.GetComponent<TurnStateUI>();

        if (stateUILogic != null)
        {
            stateUILogic.SetUpTurnState(playerTurn);
        }
    }

    /// <summary>
    /// Use to display a message to the UI
    /// </summary>
    /// <param name="messageToDisplay"></param>
    public static void DisplayMessage(string messageToDisplay) 
    {
        genericPromptUI.transform.parent.gameObject.SetActive(true);
        genericPromptUI.text = messageToDisplay;
    }

    /// <summary>
    /// Hide the generic message that appears in 'Display Message'
    /// </summary>
    public static void HideGenericMessage()
    {
        genericPromptUI.transform.parent.gameObject.SetActive(false);
    }
}

[System.Serializable]
public class TurnUIState
{
    public TurnState state;
    public GameObject uiStateHolder;
}