using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;

public class AIController : Controller
{
    public float decisionDelay;

    Player player;
    PlayerMovement movement;

    void Awake()
    {
        player = GetComponent<Player>();
        movement = GetComponent<PlayerMovement>();
        InitController();
        //GetPlayerInput().SwitchCurrentControlScheme("Keyboard&Mouse", Keyboard.current, Mouse.current);
    }

    void OnEnable()
    {
        player = GetComponent<Player>();
        player.OnPlayerTurnStateChanged += AdvanceTurnState;
    }

    void OnDisable()
    {
        player.OnPlayerTurnStateChanged -= AdvanceTurnState;
    }

    /// <summary>
    /// Get the device that this AI is assigned and have it press the submit button
    /// </summary>
    public IEnumerator OnSubmitAI()
    {
        yield return new WaitForSeconds(decisionDelay);
        print(EventSystem.current.currentSelectedGameObject);
        EventSystem.current.currentSelectedGameObject.GetComponent<Button>().onClick.Invoke();
    }

    void AdvanceTurnState(TurnState turnState)
    {
        if (turnState == TurnState.BeforeRoll)
        {
            //StartCoroutine(StartRolling());
            StartCoroutine(OnSubmitAI());
        }
        else if (turnState == TurnState.Rolling)
        {
            StartCoroutine(StartRolling());
        }
        else if (turnState == TurnState.Moving)
        {
            StartCoroutine(MoveAI());
        }
        else if (turnState == TurnState.Stopped)
        {
            StartCoroutine(OnSubmitAI());
        }
        else if (turnState == TurnState.Landed)
        {
            StartCoroutine(LandAI());
        }
    }

    public IEnumerator StartRolling(int amountToRoll = 6)
    {
        yield return new WaitForSeconds(decisionDelay);
        player.PerformRoll();
    }

    // AI Logic to Determine Where to go
    IEnumerator MoveAI()
    {
        yield return new WaitForSeconds(decisionDelay);
        List<Space> spaces = movement.currentSpace.GetPossibleSpaces(player);
        //for now, our AI is so smart that it does this randomly
        int index = Random.Range(0, spaces.Count);
        movement.MoveToSelectedSpace(spaces[index]);

        //work around for now. Will need to develop code to figure out how much stock AI should buy
        if (movement.currentSpace.GetType().Name == "Bank")
        {
            UIManager.Instance.HideStockPurchaseDisplay();
        }

        if (movement.spacesToMove > 0)
        {
            StartCoroutine(MoveAI());
        }
    }

    // AI Logic to Determine What to do when they land
    IEnumerator LandAI()
    {
        yield return new WaitForSeconds(decisionDelay);
        if (movement.currentSpace.GetType().Name == "Property")
        {
            Property property = movement.currentSpace.GetComponent<Property>();

            //change logic here to determine which button of the UI the AI should click
            if (BoardManager.CanBuySpace(player, property))
            {
                UIManager.Instance.HidePropertyPurchaseDisplay();
                BoardManager.BuySpace(player, property);
            }
        }
        else if (movement.currentSpace.GetType().Name == "RollAgain")
        {
            StartCoroutine(OnSubmitAI());
        }
    }
}
