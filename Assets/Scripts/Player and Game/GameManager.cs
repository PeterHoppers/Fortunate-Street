using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

/// <summary>
/// Pulls in information from outside the scene, plus info set by developers to set up and run the game
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Setting up an Singleton instance for the GameManager to allow spaces and other UI to request what should appear
    /// </summary>
    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    public GameObject playerObject;

    //these two will most likely be later pulled from a different scene where player creation/selection is done
    public string[] playerNames;
    public Color32[] playerColors;
    public bool[] isAI;
    public float decisionDelay;

    List<Player> players = new List<Player>();
    Player activePlayer;
    public int startingMoney;
    public int totalValueToWin;

    int baseSalary = 100;

    UIManager uiManager;

    public delegate void PlayerLevelUp(Player player);
    public static event PlayerLevelUp OnPlayerLevelUp;

    public delegate void PlayerActiveChange(Player player);
    public static event PlayerActiveChange OnPlayerActiveChange;

    // Start is called before the first frame update
    void Start()
    {
        for (int playerIndex = 0; playerIndex < playerNames.Length; playerIndex++)
        {
            GameObject newPlayer = Instantiate(playerObject);
            newPlayer.SetActive(true);
            Player playerScript = newPlayer.AddComponent<Player>();

            if (isAI[playerIndex])
            {
                AIController aiController = newPlayer.AddComponent<AIController>();
                aiController.decisionDelay = decisionDelay;
                playerScript.controller = aiController;
               
            }
            else
            {
                Controller control = newPlayer.AddComponent<HumanController>();
                playerScript.controller = control;
            }

            playerScript.input = newPlayer.GetComponent<PlayerInput>();

            playerScript.SetPlayerName(playerNames[playerIndex]);
            playerScript.SetPlayerColor(playerColors[playerIndex]);
            playerScript.SetPlayerIndex(playerIndex);

            //a test line to color each player their color
            Renderer playerRender = newPlayer.GetComponent<Renderer>();
            playerRender.material.SetColor("_Color", playerColors[playerIndex]);

            //player should start at the bank
            Space bankSpace = GameObject.FindGameObjectWithTag("Bank").GetComponent<Space>();
            BoardManager.MovePlayerToSpace(playerScript, bankSpace, false);

            //disable each of the players on start
            DisablePlayer(playerScript);

            players.Add(playerScript);
        }

        BoardManager.SetupBoard();
        MoneyManager.SetupMoney(players, startingMoney);
        SuitManager.SetupSuits(players);
        StockManager.SetupStocks(players);

        uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        if (uiManager == null)
        {
            Debug.LogError("The UI Manager is not on a gameobject called 'Canvas'");
            return;
        }

        uiManager.SetupUI(players);
        SetActivePlayer(players[0]);
    }

    /// <summary>
    /// Controlling the states of the player
    /// </summary>
    /// <param name="player"></param>
    public void PrepareRolling()
    {
        //logic to determine what amount the player should roll
        int rollAmt = 6;
        activePlayer.StartRolling(rollAmt);
    }

    public void StartMoving(Player player, int spaces)
    {
        if (player == activePlayer)
        {
            activePlayer.StartMoving(spaces);
        }
    }

    public void ContinueMoving()
    {
        activePlayer.ContinueMoving();
    }

    public void EndMoving()
    {
        activePlayer.EndMoving();
    }

    public int GetSpacesRemaining()
    {
        return activePlayer.GetSpacesLeftToMove();
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
        player.OnPlayerRolled -= StartMoving;
        player.DisablePlayer();
    }

    public void SetActivePlayer(Player player)
    {
        activePlayer = player;
        OnPlayerActiveChange?.Invoke(player);
        EventsSystemManager.Instance.UpdateActionAssetToFocusedPlayer();

        player.OnPlayerRolled += StartMoving;

        player.MakePlayerActive();
    }

    public Player GetActivePlayer()
    {
        return activePlayer;
    }

    public string GetActivePlayerName()
    {
        return activePlayer.playerName;
    }

    public void LevelUpPlayer(Player player)
    {
        //the following logic is pulled from the game, although unsure what base salary is
        int moneyGained = baseSalary + (100 * player.level) + (BoardManager.GetPlayerBoardValue(player) / 10);
        MoneyManager.MoneyChanged(player, moneyGained);
        SuitManager.ClearSuits(player);
        OnPlayerLevelUp?.Invoke(player);
        player.level++;
    }

    public bool CheckPlayerWon(Player player)
    {
        int playerTotalValue = GetPlayerTotalWorth(player);
        Debug.Log($"{player.playerName} value is {playerTotalValue}");
        //calcuate the total value of the player
        if (playerTotalValue > totalValueToWin)
        {
            Debug.Log($"{player.playerName} has won!");
        }

        return playerTotalValue > totalValueToWin;
    }

    public int GetPlayerTotalWorth(Player player)
    {
        return MoneyManager.GetPlayerMoneyValue(player)
            + StockManager.GetPlayerStockWorth(player)
            + BoardManager.GetPlayerBoardValue(player);
    }
}
