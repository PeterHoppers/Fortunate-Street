using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public Dictionary<Player, double> wallet = new Dictionary<Player, double>();
    
    // Start is called before the first frame update
    void Start()
    {
        GameManager gameManager = GameManager.GetGameManager();
        foreach (Player player in gameManager.players)
        {
            if (player != null)
            {
                wallet.Add(player, gameManager.startingMoney);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GainMoney(Player player, int amount)
    {
        wallet[player] += amount;
    }

    public void PlayerTransaction(Player payer, Player getter, int amount)
    {
        wallet[payer] -= amount;
        wallet[getter] += amount;
    }
}
