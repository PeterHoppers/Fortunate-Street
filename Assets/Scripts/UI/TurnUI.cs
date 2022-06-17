using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// To be honest, I'm not happy with this implementation of the dice UI. I feel like it'll break with ease when we allow players to undo their movement
/// It's also a lot of event calls just to keep track of where the player is moving?
/// </summary>
public class TurnUI : MonoBehaviour
{
    public TMP_Text playerName;
    public TMP_Text diceText;
    GameObject diceTextHolder;
    public Button rollButton;

    Player playerTurn;

    void OnEnable()
    {
        GameManager.OnPlayerActiveChange += PlayerChanged;
        PlayerMovement.OnPlayerRolled += DisplayDiceResults;
        Space.OnPlayerPass += ChangeDiceDisplay;
        Space.OnPlayerReverse += ChangeDiceDisplay;
        Space.OnPlayerLand += HideDiceDisplay;
    }

    void OnDisable()
    {
        GameManager.OnPlayerActiveChange -= PlayerChanged;
        PlayerMovement.OnPlayerRolled -= DisplayDiceResults;
        Space.OnPlayerPass -= ChangeDiceDisplay;
        Space.OnPlayerReverse += ChangeDiceDisplay;
        Space.OnPlayerLand -= HideDiceDisplay;
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
            playerTurn.OnPlayerTurnStateChanged -= DisplayRollingUI;
        }

        playerName.gameObject.SetActive(true);
        rollButton.gameObject.SetActive(true);
        diceTextHolder = diceText.transform.parent.gameObject;
        diceTextHolder.SetActive(false);
        playerName.text = $"{player.playerName}'s turn";

        playerTurn = player;
        playerTurn.OnPlayerTurnStateChanged += DisplayRollingUI;
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

    void DisplayRollingUI(TurnState turnState) 
    {
        if (turnState == TurnState.Rolling)
        {            
            playerName.gameObject.SetActive(false);
            rollButton.gameObject.SetActive(false);
            diceTextHolder.SetActive(true);

            //start the rolling animation
            diceText.text = "Rolling"; //yes, this is really lame for now
        }
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
    /// <summary>
    /// When the turn ends, hide the dice display since they are not moving anywhere else
    /// If we have a prompt to end the turn, this is probably where it should be activated
    /// </summary>
    /// <param name="player"></param>
    /// <param name="space"></param>
    void HideDiceDisplay(Player player, Space space)
    {
        diceTextHolder.SetActive(false);
    }
}
