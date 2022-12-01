using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAI : Player
{
    public float decisionDelay;

    void OnEnable()
    {
        this.OnPlayerTurnStateChanged += AdvanceTurnState;
    }

    void OnDisable()
    {
        this.OnPlayerTurnStateChanged -= AdvanceTurnState;
    }

    void AdvanceTurnState(TurnState turnState)
    {
        if (turnState == TurnState.BeforeRoll)
        {
            StartCoroutine(StartRolling());
        }
        else if (turnState == TurnState.Rolling)
        {
            //nothing for now, since Start Rolling moves to moving right away
        }
        else if (turnState == TurnState.Moving)
        {
            StartCoroutine(MoveAI());
        }
        else if (turnState == TurnState.Stopped)
        {
            StartCoroutine(StopAI());
        }
        else if (turnState == TurnState.Landed)
        {
            StartCoroutine(LandAI());
        }
    }

    public override IEnumerator StartRolling(int amountToRoll = 6)
    {
        SetTurnState(TurnState.Rolling);
        yield return new WaitForSeconds(decisionDelay);
        AlertRoll(Random.Range(1, amountToRoll + 1));
    }

    IEnumerator MoveAI()
    {
        yield return new WaitForSeconds(decisionDelay);
        List<Space> spaces = movement.currentSpace.GetPossibleSpaces(this);
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

    IEnumerator StopAI()
    {
        yield return new WaitForSeconds(decisionDelay);
        EndMoving();
    }

    IEnumerator LandAI()
    {
        yield return new WaitForSeconds(decisionDelay);
        if (movement.currentSpace.GetType().Name == "Property")
        {
            Property property = movement.currentSpace.GetComponent<Property>();

            if (BoardManager.CanBuySpace(this, property))
            {
                UIManager.Instance.HidePropertyPurchaseDisplay();
                BoardManager.BuySpace(this, property);
            }
            else
            {
                GameManager.Instance.AdvanceTurn();
            }
        }
        else if (movement.currentSpace.GetType().Name == "RollAgain")
        {
            GameManager.Instance.PrepareRolling();
        }
        else
        {
            GameManager.Instance.AdvanceTurn();
        }
    }
}
