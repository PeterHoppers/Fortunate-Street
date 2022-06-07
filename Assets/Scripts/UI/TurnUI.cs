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
    public Button rollButton;

    Player playerTurn;

    void OnEnable()
    {
        GameManager.OnPlayerActiveChange += PlayerChanged;
        PlayerMovement.OnPlayerRolled += DisplayDiceResults;
        Space.OnPlayerPass += ChangeDiceDisplay;
        Space.OnPlayerLand += HideDiceDisplay;
    }

    void OnDisable()
    {
        GameManager.OnPlayerActiveChange -= PlayerChanged;
        PlayerMovement.OnPlayerRolled -= DisplayDiceResults;
        Space.OnPlayerPass -= ChangeDiceDisplay;
        Space.OnPlayerLand -= HideDiceDisplay;
    }

    /// <summary>
    /// When the active player has changed, update the turn ui to match the information found on that player
    /// </summary>
    /// <param name="player"></param>
    void PlayerChanged(Player player)
    {
        playerName.gameObject.SetActive(true);
        rollButton.gameObject.SetActive(true);
        diceText.gameObject.SetActive(false);
        playerName.text = $"{player.playerName}'s turn";
        playerTurn = player;
    }

    /// <summary>
    /// Called currently through the "Roll Dice" button, although the logic to roll the dice might be better served being called somewhere else
    /// And the UI elements being hidden whenever the dice gets rolled, i.e. 'OnPlayerRolled'
    /// </summary>
    public void RollDice()
    {
        playerTurn.RollDice();
        playerName.gameObject.SetActive(false);
        rollButton.gameObject.SetActive(false);
    }

    void DisplayDiceResults(Player player, int amount)
    {
        diceText.text = amount.ToString();
        diceText.gameObject.SetActive(true);
    }

    /// <summary>
    /// This is being done to keep the UI from actually knowing what the dice results were. All we need to do is subtract one every time the player moves
    /// </summary>
    /// <param name="player"></param>
    /// <param name="space"></param>
    void ChangeDiceDisplay(Player player, Space space)
    {
        int diceAmount = int.Parse(diceText.text);
        diceAmount--;
        diceText.text = diceAmount.ToString();
    }
    /// <summary>
    /// When the turn ends, hide the dice display since they are not moving anywhere else
    /// If we have a prompt to end the turn, this is probably where it should be activated
    /// </summary>
    /// <param name="player"></param>
    /// <param name="space"></param>
    void HideDiceDisplay(Player player, Space space)
    {
        diceText.gameObject.SetActive(false);
    }
}
