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

    int indexInSection;
    int spacesToMove;
    int spaceInSection;

    Player player;
    // Start is called before the first frame update
    void Start()
    {
        Space bankSpace = GameObject.FindGameObjectWithTag("Bank").GetComponent<Space>();
        TeleportToSpace(bankSpace);
    }

    // Update is called once per frame
    void Update()
    {
        //mocking up some controls
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //if we aren't moving anywhere, roll
            if (spacesToMove == 0)
            {
                spacesToMove = RollDice();
                Debug.Log($"How many spaces we're moving {spacesToMove}");
            }
            else
            {
                Intersection intersection = currentSpace.GetComponent<Intersection>();
                if (intersection != null)
                {
                    MoveToNewSection(intersection); //possibly make this a courintine until the user selects where to go
                }
                else 
                {
                    MoveNextSpace();
                }                
                    
                spacesToMove--;

                if (spacesToMove == 0)
                {
                    currentSpace.OnPlayerLand(player);
                }
                else
                {
                    currentSpace.OnPlayerPass(player);
                }                                  
            }
            
        }
    }

    public void SetPlayer(Player player)
    {
        this.player = player;
    }

    public int RollDice()
    {
        return UnityEngine.Random.Range(1, 7);
    }

    public void TeleportToSpace(Space space)
    {
        MoveToSpace(space);
        currentBoardSection = BoardManager.GetBoardSectionOfSpace(space); //only need to figure this out when we drastically move across the board
        indexInSection = Array.IndexOf(currentBoardSection.spaces, space); //figure out where in the array we are for the board section
    }

    public void MoveToSpace(Space space)
    { 
        player.transform.position = new Vector3(space.transform.position.x, 1, space.transform.position.z);
        currentSpace = space;        
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

        Space targetSpace = currentBoardSection.spaces[indexInSection];

        MoveToSpace(targetSpace);
    }

    public void MoveToNewSection(Intersection intersection)
    {
        //TODO: we need to remove the space we just came from as an option to move to
        List<BoardSection> possibleSections = intersection.connectingSections.ToList();
        List<Space> possibleTargetSpaces = intersection.connectedSpaces.ToList();
        /*
         * 
        int currentIndexOfSection = possibleSections.IndexOf(currentBoardSection);

        if (currentIndexOfSection > -1)
        {
            possibleSections.RemoveAt(currentIndexOfSection);
            possibleTargetSpaces.RemoveAt(currentIndexOfSection);
        }

        if (possibleTargetSpaces.Contains(currentSpace))
        {
            possibleTargetSpaces.Remove(currentSpace);
        }*/

        //if there's only one possible section, move to it
        if (possibleSections.Count == 1)
        {
            currentBoardSection = possibleSections[0];
            MoveToSpace(possibleTargetSpaces[0]);
        }
        else  //have the user choose which direction to go
        {
            int targetDirection = UnityEngine.Random.Range(0, possibleSections.Count);
            currentBoardSection = possibleSections[targetDirection];
            Space targetSpace = possibleTargetSpaces[targetDirection];
            indexInSection = Array.IndexOf(currentBoardSection.spaces, targetSpace);

            //say the interaction is at 0 for section, we need to keep moving forward in the section
            if (indexInSection == 0)
            {
                isForward = true;
            }
            else if (indexInSection == currentBoardSection.spaces.Length - 1)
            {
                isForward = false;
            }

            MoveToSpace(targetSpace);
        }
    }
}
