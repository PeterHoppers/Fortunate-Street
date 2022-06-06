using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject allPlayerStatsPanel;
    public GameObject statsPanel;

    // Start is called before the first frame update
    void OnEnable()
    {
        Space.OnPlayerPass += ReportMovement;
        MoneyManager.OnPlayerMoneyChanged += UpdateMoneyUI;
    }

    void OnDisable()
    {
        Space.OnPlayerPass -= ReportMovement;
        MoneyManager.OnPlayerMoneyChanged -= UpdateMoneyUI;
    }

    public void SetupUI(List<Player> players)
    {
        foreach (Player player in players)
        {
            if (player != null)
            {
                GameObject statPanel = Instantiate(statsPanel, allPlayerStatsPanel.transform);
                PlayerOverviewUI overview = statPanel.GetComponent<PlayerOverviewUI>();

                if (overview == null)
                {
                    Debug.LogError("Game object provided for player overview does not have the right script attached");
                    return;
                }

                overview.SetPlayer(player);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// More or less an example method to test events
    /// </summary>
    /// <param name="player"></param>
    /// <param name="space"></param>
    void ReportMovement(Player player, Space space)
    {
        string name = space.spaceName;

        if (name == "")
        {
            name = space.name;
        }

        Debug.Log($"{player.playerName} has passed {name}");
    }

    void UpdateMoneyUI(Player player, int oldAmount, int newAmount)
    {
        Debug.Log($"{player.playerName}'s money has gone from {oldAmount} to {newAmount}");
    }

}
