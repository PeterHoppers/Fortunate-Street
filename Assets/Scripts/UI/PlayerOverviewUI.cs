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
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }
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
        SetPlayerMoney(MoneyManager.GetPlayerMoneyValue(player));

        GetComponent<Image>().color = player.color;
    }

    void SetPlayerMoney(int amount)
    {
        playerMoney.text = $"$ {amount}";
    }


    public void UpdatePlayerMoney(Player player, int oldAmount, int newAmount)
    {        
        if (player != referredPlayer)
        {
            return;
        }

        Debug.Log($"{player.playerName}'s money has gone from {oldAmount} to {newAmount}");

        if (oldAmount < newAmount)
        {
            Debug.Log("Go up!");
            animator.Play("gainmoney");
        } 
        else if (oldAmount > newAmount)
        {
            Debug.Log("Go down!");
            animator.Play("losemoney");
        }

        SetPlayerMoney(newAmount);
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
