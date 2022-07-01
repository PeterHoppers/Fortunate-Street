using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

/// <summary>
/// To be honest, I'm not happy with this implementation of the dice UI. I feel like it'll break with ease when we allow players to undo their movement
/// It's also a lot of event calls just to keep track of where the player is moving?
/// </summary>
public class TurnUI : MonoBehaviour
{
    public TurnUIState[] uiStates;
    public TMP_Text playerName;
    public TMP_Text diceText;
    public Button rollButton;

    Player playerTurn;

    void OnEnable()
    {
        GameManager.OnPlayerActiveChange += PlayerChanged;
        PlayerMovement.OnPlayerRolled += DisplayDiceResults;
        Space.OnPlayerPass += ChangeDiceDisplay;
        Space.OnPlayerReverse += ChangeDiceDisplay;
    }

    void OnDisable()
    {
        GameManager.OnPlayerActiveChange -= PlayerChanged;
        PlayerMovement.OnPlayerRolled -= DisplayDiceResults;
        Space.OnPlayerPass -= ChangeDiceDisplay;
        Space.OnPlayerReverse += ChangeDiceDisplay;
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

        playerName.text = $"{player.playerName}'s turn";
        playerTurn = player;
        playerTurn.OnPlayerTurnStateChanged += DisplayStateUI;
        DisplayStateUI(TurnState.BeforeRoll);
    }

    /// <summary>
    /// Called currently through the "Roll Dice" button, although the logic to roll the dice might be better served being called somewhere else
    /// And the UI elements being hidden whenever the dice gets rolled, i.e. 'OnPlayerRolled'
    /// This is a call here, since there are other places that might allow the player to start rolling, like chance spaces or the roll again space
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
        //deactivate all other state UI
        foreach (TurnUIState turnUIState in uiStates)
        {
            turnUIState.uiStateHolder.SetActive(false);
        }

        TurnUIState stateUIHolder = uiStates.Where(ui => ui.state == turnState).FirstOrDefault();

        if (stateUIHolder == null)
        {
            return;
        }

        stateUIHolder.uiStateHolder.SetActive(true);
    }

    void DisplayDiceResults(Player player, int amount)
    {
        diceText.text = amount.ToString();
    }

    /// <summary>
    /// This is being done to keep the UI from actually knowing what the dice results were. All we need to do is subtract one every time the player moves
    /// </summary>
    /// <param name="player"></param>
    /// <param name="space"></param>
    void ChangeDiceDisplay(Player player, Space space = null)
    {
        diceText.text = player.GetSpacesRemaining().ToString();
    }
}

[System.Serializable]
public class TurnUIState
{
    public TurnState state;
    public GameObject uiStateHolder;
}