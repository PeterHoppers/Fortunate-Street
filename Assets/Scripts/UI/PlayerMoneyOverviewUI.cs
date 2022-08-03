using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerMoneyOverviewUI : MonoBehaviour
{
    public TMP_Text playerName;
    public TMP_Text playerMoney;
    public TMP_Text playerStocks;
    public TMP_Text playerTotal;

    Image image;

    void Start()
    {
        image = GetComponent<Image>();
    }

    public void DisplayPlayer(Player player) 
    {
        image.color = player.color;
        playerName.text = player.playerName;
        playerMoney.text = MoneyManager.wallet[player].ToString();
        playerStocks.text = StockManager.GetPlayerStockWorth(player).ToString();
        playerTotal.text = Camera.main.GetComponent<GameManager>().GetPlayerTotalWorth(player).ToString();
    }
}
