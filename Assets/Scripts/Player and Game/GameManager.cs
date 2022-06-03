using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Pulls in information from outside the scene, plus info set by developers to set up and run the game
/// </summary>
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
    public Player activePlayer;
    public int startingMoney;
    public int totalValueToWin;

    int baseSalary = 100;

    // Start is called before the first frame update
    void Awake()
    {
        players = FindObjectsOfType<Player>();
        Debug.Log($"Found {players.Length} players");

        BoardManager.SetupBoard();
        MoneyManager.SetupMoney(players, startingMoney);
        SuiteManager.SetupSuites(players);
        StockManager.SetupStocks();        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LevelUpPlayer(Player player)
    {
        //the following logic is pulled from the game, although unsure what base salary is
        int moneyGained = baseSalary + (100 * player.level) + (BoardManager.GetPlayerShops(player).Count / 10);
        MoneyManager.GainMoney(player, moneyGained);
        player.level++;
    }

    public void CheckPlayerWon(Player player)
    {
        int playerTotalValue = MoneyManager.GetPlayerMoneyValue(player)
            + StockManager.GetPlayerStockWorth(player)
            + BoardManager.GetPlayerBoardValue(player);
        //calcuate the total value of the player
        if (playerTotalValue > totalValueToWin)
        {
            Debug.Log("Player has won!");
        }
    }
}
