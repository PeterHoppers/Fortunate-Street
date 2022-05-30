using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public double startingAmt;
    Player activePlayer;
    double totalValueToWin;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
