using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RollUI : MonoBehaviour
{
    public TMP_Text diceText;

    // Start is called before the first frame update
    void OnEnable()
    {
        Dice.OnPlayerRolled += DisplayDiceResults;
        Space.OnPlayerPass += DecrementDiceDisplay;
        Space.OnPlayerReverse += IncrementDiceDisplay;
    }

    void OnDisable()
    {
        Dice.OnPlayerRolled -= DisplayDiceResults;
        Space.OnPlayerPass -= DecrementDiceDisplay;
        Space.OnPlayerReverse -= IncrementDiceDisplay;
    }

    void DisplayDiceResults(Player player, int amount)
    {
        UpdateDiceDisplay(amount);
    }

    /// <summary>
    /// This is being done to keep the UI from actually knowing what the dice results were. All we need to do is subtract one every time the player moves
    /// </summary>
    /// <param name="player"></param>
    /// <param name="space"></param>
    void DecrementDiceDisplay(Player player, Space space = null)
    {
        int diceNumber = int.Parse(diceText.text);
        diceNumber--;
        UpdateDiceDisplay(diceNumber);
    }

    void IncrementDiceDisplay(Player player, Space space = null)
    {
        int diceNumber = int.Parse(diceText.text);
        diceNumber++;
        UpdateDiceDisplay(diceNumber);
    }

    void UpdateDiceDisplay(int diceAmount)
    {
        diceText.text = diceAmount.ToString();
    }
}
