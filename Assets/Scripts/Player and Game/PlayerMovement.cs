using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Space currentSpace;
    public BoardSection currentBoardSection;
    public bool isForward; //used for determining the direction they are going through a section

    public float moveSpeed = 1f;

    int indexInSection;
    int spacesToMove;
    float heightOffset = .6f; //half the player's height + the height of the spaces

    Player player;

    public delegate void PlayerRolled(Player player, int spaces);
    public static event PlayerRolled OnPlayerRolled;

    // Start is called before the first frame update
    void Start()
    {
        //player should start at the bank
        Space bankSpace = GameObject.FindGameObjectWithTag("Bank").GetComponent<Space>();
        TeleportToSpace(bankSpace);
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
                return;
            }

            Debug.Log($"{player.playerName} wants to move {spacesToMove} spaces.");

            //if we aren't moving anywhere, don't allow the player to move
            if (spacesToMove == 0)
            {
                return;
            }

            //we might need to restructure this part of the code so that it returns a list of options to move to
            //then the user selects which one to go to
            //since while the player is normally walking around the board they have the option to go backwards?
            Intersection intersection = currentSpace.GetComponent<Intersection>();
            if (intersection != null)
            {
                MoveToNewSection(intersection); 
            }
            else 
            {
                MoveNextSpace();
            }                
                    
            spacesToMove--;

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
    public void TeleportToSpace(Space space, bool hasTeleported = false)
    {
        MoveToSpace(space);
        currentBoardSection = BoardManager.GetBoardSectionOfSpace(space);
        indexInSection = Array.IndexOf(currentBoardSection.spaces, space); //figure out where in the array we are for the board section

        //we might need a better system here to figure out if the player is now moving forwards or backwards through a section
        Debug.Log($"Index in new section: {indexInSection}");
        isForward = (indexInSection < currentBoardSection.spaces.Length / 2);

        /* Old way of doing this. Issue popped up when an intersection was the last or first element of board section
            * and then moved the player onto the 2nd to last or 2nd element of the board section to move them along that section
        if (indexInSection == 0)
        {
            isForward = true;
        }
        else if (indexInSection == currentBoardSection.spaces.Length - 1)
        {
            isForward = false;
        }
        */
    }

    public void MoveToSpace(Space space)
    {
        //save the previous space as the space entered from
        if (space.GetComponent<Intersection>())
        {
            space.GetComponent<Intersection>().EnterIntersection(player, currentSpace);
        }

        // === OLD CODE === \\
        //this should probably be redone to lerp the player to the position to create a smoother transition
        //or use an animation that takes in a destination Vector 3
        //player.transform.position = new Vector3(space.transform.position.x, heightOffset, space.transform.position.z);

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

    public void MoveNextSpace()
    {
        //increase the index if we're moving forward through the section, decrease if we're moving backwards
        if (isForward)
        {
            indexInSection++;
        }
        else 
        {
            indexInSection--;
        }

        //we should probably throw an error that mentions the board has been set up improperly if we get an OutOfRange error here
        Space targetSpace = currentBoardSection.spaces[indexInSection];

        MoveToSpace(targetSpace);
    }

    public void MoveToNewSection(Intersection intersection)
    {
        List<Space> possibleTargetSpaces = intersection.GetPossibleSpaces(player);

        Debug.Log("Possible options:");
        foreach (Space possibleSpace in possibleTargetSpaces)
        {
            Debug.Log($"Space: {possibleSpace}");
        }

        //if removing the previous space removes all possible target spaces, treat this like a normal space
        if (possibleTargetSpaces.Count == 0)
        {
            MoveNextSpace();
                     
        }
        //if there's only one possible section, move to it
        else if (possibleTargetSpaces.Count == 1)
        {
            TeleportToSpace(possibleTargetSpaces[0]);
        }
        else  
        {
            int targetDirection = UnityEngine.Random.Range(0, possibleTargetSpaces.Count); //have the user choose which direction to go, rather than random
            Space targetSpace = possibleTargetSpaces[targetDirection];
            TeleportToSpace(targetSpace);            
        }

        intersection.LeaveIntersection(player);
    }
}
