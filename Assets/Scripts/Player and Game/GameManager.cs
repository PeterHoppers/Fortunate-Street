using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Pulls in information from outside the scene, plus info set by developers to set up and run the game
/// </summary>
public class GameManager : MonoBehaviour
{
    public GameObject playerObject;

    //these two will most likely be later pulled from a different scene where player creation/selection is done
    public string[] playerNames;
    public Color32[] playerColors;

    List<Player> players = new List<Player>();
    Player activePlayer;
    public int startingMoney;
    public int totalValueToWin;

    int baseSalary = 100;

    UIManager uiManager;

    public delegate void PlayerLevelUp(Player player);
    public static event PlayerLevelUp OnPlayerLevelUp;

    // Start is called before the first frame update
    void Awake()
    {
        //players = FindObjectsOfType<Player>();
        //Debug.Log($"Found {players.Length} players");

        for (int playerIndex = 0; playerIndex < playerNames.Length; playerIndex++)
        {
            GameObject newPlayer = Instantiate(playerObject);
            Player newPlayerScript = newPlayer.GetComponent<Player>();
            
            newPlayerScript.playerName = playerNames[playerIndex];
            newPlayerScript.color = playerColors[playerIndex];
            newPlayerScript.id = playerIndex;

            //a test line to color each player their color
            Renderer playerRender = newPlayer.GetComponent<Renderer>();
            playerRender.material.SetColor("_Color", playerColors[playerIndex]);

            //disable each of the players on start
            DisablePlayer(newPlayerScript);

            players.Add(newPlayerScript);
        }

        SetActivePlayer(players[0]);

        BoardManager.SetupBoard();
        MoneyManager.SetupMoney(players, startingMoney);
        SuiteManager.SetupSuites(players);
        StockManager.SetupStocks();

        uiManager = GetComponent<UIManager>();

        if (uiManager == null)
        {
            Debug.LogError("The UI Manager is not on the same game object as the game manager.");
            return;
        }

        uiManager.SetupUI(players);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckPlayerLevelUp(Player player)
    {
        int suiteCount = SuiteManager.suites[player].Count;
        Debug.Log($"Sutie count is: {suiteCount}");
        //we also need to check for suite yourself cards

        if (suiteCount >= 4)
        {
            LevelUpPlayer(player);
            Debug.Log("Leveled up!");
        }
    }

    public void LevelUpPlayer(Player player)
    {
        //the following logic is pulled from the game, although unsure what base salary is
        int moneyGained = baseSalary + (100 * player.level) + (BoardManager.GetPlayerBoardValue(player) / 10);
        MoneyManager.MoneyChanged(player, moneyGained);
        SuiteManager.ClearSuites(player);
        OnPlayerLevelUp?.Invoke(player);
        player.level++;
    }

    public void CheckPlayerWon(Player player)
    {
        int playerTotalValue = MoneyManager.GetPlayerMoneyValue(player)
            + StockManager.GetPlayerStockWorth(player)
            + BoardManager.GetPlayerBoardValue(player);
        Debug.Log($"{player.playerName} value is {playerTotalValue}");
        //calcuate the total value of the player
        if (playerTotalValue > totalValueToWin)
        {
            Debug.Log($"{player.playerName} has won!");
        }
    }

    public void AdvanceTurn()
    {
        DisablePlayer(activePlayer);

        int activePlayerId = activePlayer.id;
        activePlayerId++;

        if (activePlayerId >= players.Count)
        {
            activePlayerId = 0;
        }

        SetActivePlayer(players[activePlayerId]);
    }

    public void DisablePlayer(Player player)
    {
        player.StopPlayerActive();
    }

    public void SetActivePlayer(Player player)
    {
        player.MakePlayerActive();
        activePlayer = player;
    }
}
