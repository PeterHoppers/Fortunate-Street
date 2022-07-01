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
            int rolled = Random.Range(1, maxNumber + 1);
            OnPlayerRolled?.Invoke(rollingPlayer, rolled);
            rollingPlayer = null;
        }
    }
}
