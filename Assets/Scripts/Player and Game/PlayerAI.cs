using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAI : Player
{
    public float decisionDelay;

    void OnEnable()
    {
        Dice.OnPlayerRolled += StartMoving;
        this.OnPlayerTurnStateChanged += AdvanceTurnState;
    }

    // Update is called once per frame
    void OnDisable()
    {
        Dice.OnPlayerRolled -= StartMoving;
        this.OnPlayerTurnStateChanged -= AdvanceTurnState;
    }

    // Start is called before the first frame update
    void Start()
    {
        dice = Camera.main.GetComponent<Dice>();
    }

    void AdvanceTurnState(TurnState turnState)
    {
        if (turnState == TurnState.BeforeRoll)
        {
            SetTurnState(TurnState.Rolling);
        }
        else if (turnState == TurnState.Rolling)
        {
            StartCoroutine(RollAI());
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

    IEnumerator RollAI()
    {
        yield return new WaitForSeconds(decisionDelay);
        dice.SetupRoll(this, 6);
        yield return new WaitForSeconds(decisionDelay);
        dice.RollDice();
    }

    IEnumerator MoveAI()
    {
        yield return new WaitForSeconds(decisionDelay);
        List<Space> spaces = movement.currentSpace.GetPossibleSpaces(this);
        //for now, our AI is so smart that it does this randomly
        int index = Random.Range(0, spaces.Count);
        movement.MoveToSelectedSpace(spaces[index]);

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
            Property landed = movement.currentSpace.GetComponent<Property>();

            if (landed.CanBuyProperty(this))
            {
                landed.BuySpace(this);
            }
            else
            {
                SetTurnState(TurnState.Finished);
            }
        }
        else
        {
            //right now, there's a bug with the Roll Again space not working without the UI for it, since the turn doesn't finish when they land on it
            if (movement.currentSpace.GetType().Name != "RollAgain")
            {
                SetTurnState(TurnState.Finished);
            }
            else 
            {
                StartCoroutine(RollAI());
            }
        }
    }
}
