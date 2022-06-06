using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerOverviewUI : MonoBehaviour
{
    public TMP_Text playerName;
    public TMP_Text playerMoney;
    public SuiteUI[] playerSuites;

    Player referredPlayer;

    // Start is called before the first frame update
    void OnEnable()
    {
        MoneyManager.OnPlayerMoneyChanged += UpdatePlayerMoney;
        SuiteManager.OnPlayerGotSuite += UpdateSuiteUI;
        GameManager.OnPlayerLevelUp += ClearSuiteUI;
    }

    void OnDisable()
    {
        MoneyManager.OnPlayerMoneyChanged -= UpdatePlayerMoney;
        SuiteManager.OnPlayerGotSuite -= UpdateSuiteUI;
        GameManager.OnPlayerLevelUp -= ClearSuiteUI;
    }

    public void SetPlayer(Player player)
    {
        referredPlayer = player;
        playerName.text = player.playerName;
        playerMoney.text = MoneyManager.GetPlayerMoneyValue(player).ToString();

        GetComponent<Image>().color = player.color;
    }

    public void UpdatePlayerMoney(Player player, int oldAmount, int newAmount)
    {
        if (player != referredPlayer)
        {
            return;
        }

        playerMoney.text = newAmount.ToString();
    }

    public void UpdateSuiteUI(Player player, Suite suite)
    {
        if (player != referredPlayer)
        {
            return;
        }

        foreach (SuiteUI suiteUI in playerSuites)
        {
            if (suiteUI.representingSuite == suite)
            {
                suiteUI.GotSuite();
            }
        }
    }

    public void ClearSuiteUI(Player player)
    {
        if (player != referredPlayer)
        {
            return;
        }

        foreach (SuiteUI suiteUI in playerSuites)
        {
            suiteUI.SpentSuite();
        }
    }
}
