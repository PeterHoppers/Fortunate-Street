using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameManager()
    { }

    private static readonly GameManager gameManager = new GameManager();
    public static GameManager GetGameManager()
    {
        return gameManager;
    }

    public Player[] players;
    public double startingMoney;
    public Player activePlayer;
    double totalValueToWin;

    int baseSalary;

    // Start is called before the first frame update
    void Awake()
    {
        BoardManager.SetupBoard();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LevelUpPlayer(Player player)
    {
        int moneyGained = baseSalary + (100 * player.level) + (BoardManager.GetPlayerShops(player).Count / 10);
        MoneyManager.GainMoney(player, moneyGained);
        player.level++;
    }

    public bool HasPlayerWon(Player player)
    {
        int playerTotalValue = 0;
        //calcuate the total value of the player
        return (playerTotalValue > totalValueToWin);
    }
}
