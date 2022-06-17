using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Space currentSpace;
    bool canMoveAnyDirection; //used for determining the direction they are going through a section
    bool startingAnyDirection;

    public float moveSpeed = 1f;

    public int spacesToMove;
    float heightOffset = .6f; //half the player's height + the height of the spaces

    //going back logic
    Stack<Space> visitedSpaces = new Stack<Space>();

    Player player;

    public delegate void PlayerRolled(Player player, int spaces);
    public static event PlayerRolled OnPlayerRolled;

    // Start is called before the first frame update
    void Start()
    {
        //player should start at the bank
        Space bankSpace = GameObject.FindGameObjectWithTag("Bank").GetComponent<Space>();
        currentSpace = bankSpace;
        player.transform.position = new Vector3(bankSpace.transform.position.x, heightOffset, bankSpace.transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        //mocking up some controls
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (player.GetTurnState() == TurnState.Rolling)
            {
                PlayerRollDice();

                if (currentSpace.HasPlayerEntered(player))
                {
                    canMoveAnyDirection = false;
                    startingAnyDirection = false;
                }
                else
                {
                    canMoveAnyDirection = true;
                    startingAnyDirection = true;
                }

                Debug.Log($"At the start of this turn, we can move in any direction? {startingAnyDirection}");

                return;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Backspace))
        {
            ReverseMovement();
        }
    }

    public void SetPlayer(Player player)
    {
        this.player = player;
    }

    public void PlayerRollDice(int maxNumber = 6)
    {
        spacesToMove = RollDice(maxNumber);
        player.SetTurnState(TurnState.Moving);
        Debug.Log($"How many spaces we're moving {spacesToMove}");
    }

    int RollDice(int maxNumber = 6)
    {
        int rolled = UnityEngine.Random.Range(1, maxNumber + 1);
        OnPlayerRolled?.Invoke(player, rolled);
        return rolled;
    }


    /// <summary>
    /// Needs a new name. Called when the player teleports or makes a decision at an intersection. Contains logic for moving and detecting if the 
    /// player has switched board sections. Whenever the player might have switched sections, it is better to call this than MoveToSpace()
    /// </summary>
    /// <param name="space"></param>
    /// <param name="hasTeleported"></param>
    public void TeleportToSpace(Space space)
    {
        currentSpace = space;
        space.PlayerLanded(player);
        player.transform.position = new Vector3(space.transform.position.x, heightOffset, space.transform.position.z);
    }

    public void MoveToSpace(Space space)
    {
        //save the previous space as the space entered from
        space.EnterSpaceFrom(player, currentSpace);

        // === NEW CODE === \\
        StartCoroutine(LerpPosition(space.transform.position, (1 / moveSpeed)));

        currentSpace = space;        
    }

    // === Lerp the player forward for smoother movement === \\
    IEnumerator LerpPosition(Vector3 targetSpace, float duration)
    {
        Vector3 newSpace = new Vector3(targetSpace.x, heightOffset, targetSpace.z);
        {
            float time = 0;
            Vector3 startPosition = transform.position;
            while (time < duration)
            {
                transform.position = Vector3.Lerp(startPosition, newSpace, time / duration);
                time += Time.deltaTime;
                yield return null;
            }
            transform.position = newSpace;
        }
    }

    public List<Space> GetPossibleMovementSpots()
    {
        Debug.Log($"We're getting the possible spaces for {currentSpace}");
        if (canMoveAnyDirection)
        {
            return currentSpace.connectedSpaces.ToList();
        }
        else
        {
            return currentSpace.GetPossibleSpaces(player);
        }
    }

    public void MoveToSelectedSpace(Space targetSpace)
    {
        //if we're trying to move to where we just were, we're moving backwards instead
        if (visitedSpaces.Count > 0 && targetSpace == visitedSpaces.Peek())
        {
            ReverseMovement();
            return;
        }
        //remove player from the previous space
        visitedSpaces.Push(currentSpace);
        currentSpace.LeaveSpace(player);

        MoveToSpace(targetSpace);
        spacesToMove--;
        //now that we've moved, allow us to move back the way we came
        //canMoveAnyDirection = true;

        if (spacesToMove == 0)
        {
            player.SetTurnState(TurnState.Landed);
            currentSpace.PlayerLanded(player);
        }
        else
        {
            currentSpace.PlayerPassed(player);
        }
    }

    public void ReverseMovement()
    {
        Debug.Log($"Let's go back!");
        //grab the previous space we just were
        if (visitedSpaces.Count == 0)
        {
            //if there isn't a previous space, don't go backwards
            return;
        }

        spacesToMove++;
        currentSpace.LeaveSpace(player);

        Space returningSpace = visitedSpaces.Pop();
        Debug.Log($"Returning space is {returningSpace.spaceName}");
        //move the player to that space
        MoveToSpace(returningSpace);
        //undo the logic for passing the space
        returningSpace.PlayerReversed(player);

        //if we're back at the start
        if (visitedSpaces.Count == 0)
        {
            returningSpace.LeaveSpace(player);
            canMoveAnyDirection = startingAnyDirection;
        }
    }
}
