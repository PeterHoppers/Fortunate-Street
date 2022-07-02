using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    public delegate void PlayerRolled(Player player, int spaces);
    public static event PlayerRolled OnPlayerRolled;

    Player rollingPlayer;
    int maxNumber = 6;

    public void SetupRoll(Player player, int maxNumber)
    {
        rollingPlayer = player;
        this.maxNumber = maxNumber;
    }

    public int RollDice()
    {
        int rolled = Random.Range(1, maxNumber + 1);
        OnPlayerRolled?.Invoke(rollingPlayer, rolled);
        rollingPlayer = null;
        return rolled;
    }

    /// <summary>
    /// Used for debugging at the moment, or maybe for when a chance thing makes you roll a specific number
    /// </summary>
    /// <param name="rolledAmt"></param>
    /// <returns></returns>
    int CheatDice(int rolledAmt)
    {
        int rolled = rolledAmt;
        OnPlayerRolled?.Invoke(rollingPlayer, rolled);
        rollingPlayer = null;
        return rolled;
    }

    // Update is called once per frame
    void Update()
    {
        //if no one is rolling, don't check for input
        if (rollingPlayer == null)
        {
            return;
        }

        //mocking up some controls
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RollDice();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            CheatDice(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            CheatDice(2);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            CheatDice(3);
        }
    }
}
