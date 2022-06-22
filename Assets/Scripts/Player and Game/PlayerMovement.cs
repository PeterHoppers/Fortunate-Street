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

    //logic for matching keys to movement
    public Dictionary<KeyCode, Space> keysSpaces = new Dictionary<KeyCode, Space>();
    Dictionary<Vector3, KeyCode> keyDirections = new Dictionary<Vector3, KeyCode>()
        {
            {Vector3.forward, KeyCode.RightArrow},
            {Vector3.back, KeyCode.LeftArrow},
            {Vector3.left, KeyCode.UpArrow},
            {Vector3.right, KeyCode.DownArrow}
        };

    //going back logic
    Stack<Space> visitedSpaces = new Stack<Space>();

    Player player;
    PlayerMovementUI ui;
    TurnState playerTurnState;

    public delegate void PlayerRolled(Player player, int spaces);
    public static event PlayerRolled OnPlayerRolled;

    //caled on Awake from Player
    public void SetPlayer(Player player)
    {
        this.player = player;
        ui = GetComponent<PlayerMovementUI>();
    }

    void OnEnable()
    {
        player.OnPlayerTurnStateChanged += UpdateTurnState;
    }
    void OnDisable()
    {
        player.OnPlayerTurnStateChanged -= UpdateTurnState;
    }

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
        if (playerTurnState == TurnState.Rolling)
        {
            //mocking up some controls
            if (Input.GetKeyDown(KeyCode.Space))
            {
                PlayerRollDice();
                visitedSpaces.Clear();

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

                SetupMovementOptions();

                Debug.Log($"At the start of this turn, we can move in any direction? {startingAnyDirection}");

                return;
            }
        }
        else if (playerTurnState == TurnState.Moving)
        {
            foreach (KeyCode key in keysSpaces.Keys)
            {
                if (Input.GetKeyDown(key))
                {
                    MoveToSelectedSpace(keysSpaces[key]);
                    SetupMovementOptions();
                    break;
                }
            }

            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                ReverseMovement();
            }
        }
    }

    public void UpdateTurnState(TurnState turnState)
    {
        playerTurnState = turnState;
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

    void SetupMovementOptions()
    {
        //grab all the spaces we can move
        List<Space> spacesToGo = GetPossibleMovementSpots();
        //List<Space> spacesToGo = currentSpace.GetPossibleSpaces(player);
        keysSpaces.Clear();
        // == Clear old Arrow objects before making new ones == \\
        ui.RemoveOldArrows();

        foreach (Space space in spacesToGo)
        {
            //right now, forward is to the right
            Vector3 directionToSpace = space.transform.position - currentSpace.transform.position;

            ui.SpawnArrowForSpace(currentSpace, space, directionToSpace);            

            

            if (keyDirections.ContainsKey(directionToSpace))
            {
                keysSpaces.Add(keyDirections[directionToSpace], space);
            }
            else
            {                
                //use the zOffset to figure out if we're going forwards or backwards
                Vector3 offset = space.transform.position - currentSpace.transform.position;
                float zOffset = offset.z;
                float xOffset = offset.x;

                //these x and z offsets give you the coordinates the next space is at
                // z is up and down
                // x is left and right
                //if mutliple spaces want to try and do the same angle, for now,
                //remove that key as a valid option to move so that the keys that are different remain
                if (zOffset < 0)
                {
                    CheckThenAddSpace(Vector3.back, space);
                }
                else if (zOffset > 0)
                {
                    CheckThenAddSpace(Vector3.forward, space);
                }

                if (xOffset < 0)
                {
                    CheckThenAddSpace(Vector3.left, space);
                }
                else if (xOffset > 0)
                {
                    CheckThenAddSpace(Vector3.right, space);
                }
            }
        }
    }

    void CheckThenAddSpace(Vector3 vectorCheck, Space space)
    {
        KeyCode attemptedButton = keyDirections[vectorCheck];
        if (keysSpaces.ContainsKey(attemptedButton))
        {
            keysSpaces.Remove(attemptedButton);
        }
        else
        {
            keysSpaces.Add(attemptedButton, space);
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
        canMoveAnyDirection = true;

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
            canMoveAnyDirection = startingAnyDirection;
        }
    }
}
