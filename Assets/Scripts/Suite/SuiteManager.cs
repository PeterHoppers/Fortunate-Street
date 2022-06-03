using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuiteManager : MonoBehaviour
{
    public static Dictionary<Player, List<Suite>> suites = new Dictionary<Player, List<Suite>>();
    public const int suitMoneyValue = 100;
    // Start is called before the first frame update
    void Start()
    {
        //foreach Player, add them to the suites dictionary
        foreach (Player player in GameManager.GetGameManager().players) 
        {
            if (player != null)
            {
                suites.Add(player, new List<Suite>());
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void GetSuite(Player player, Suite suite)
    {
        if (!suites[player].Contains(suite))
        {
            suites[player].Add(suite);
        }
        else 
        {
            //the player gains money according to suitMoneyValue
            MoneyManager.GainMoney(player, suitMoneyValue);
        }
    }

    public static void ClearSuites(Player player)
    {
        suites[player] = new List<Suite>();
    }
}
