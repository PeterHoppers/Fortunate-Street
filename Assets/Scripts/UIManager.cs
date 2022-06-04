using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
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

        Debug.Log($"{player.name} has passed {name}");
    }

    void UpdateMoneyUI(Player player, int oldAmount, int newAmount)
    {
        Debug.Log($"{player.name}'s money has gone from {oldAmount} to {newAmount}");
    }

}
