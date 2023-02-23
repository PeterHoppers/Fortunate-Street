using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Space currentSpace;
    Space spaceLastTurnCameFrom; //saves the information of where the player came from at the end of their last turn

    public float moveSpeed = 1f;

    public int spacesToMove;
    float heightOffset = .6f; //half the player's height + the height of the spaces

    //logic for matching keys to movement
    public Dictionary<SpaceDirection, Space> keysSpaces = new Dictionary<SpaceDirection, Space>();
    Dictionary<Vector3, SpaceDirection> keyDirections = new Dictionary<Vector3, SpaceDirection>()
        {
            {Vector3.forward, SpaceDirection.Right},
            {Vector3.back, SpaceDirection.Left},
            {Vector3.left, SpaceDirection.Up},
            {Vector3.right, SpaceDirection.Down}
        };

    //going back logic
    Stack<Space> visitedSpaces = new Stack<Space>();

    Player player;
    PlayerMovementUI ui;

    //caled on Awake from Player
    public void SetPlayer(Player player)
    {
        this.player = player;
        ui = GetComponent<PlayerMovementUI>();
    }

    public void SelectSpace(SpaceDirection direction)
    {
        if (spacesToMove == 0)
        {
            return;
        }

        if (!keysSpaces.ContainsKey(direction))
        {
            return;
        }

        Space targetSpace = keysSpaces[direction];
        if (visitedSpaces.Count > 0 && targetSpace == visitedSpaces.Peek())
        {
            ReverseMovement(); //since we can only go back to one space, we don't need to pass it in here
        }
        else
        {
            MoveToSelectedSpace(targetSpace);
        }

        SetupMovementOptions();
    }

    public void SetupMovementOptions()
    {
        //grab all the spaces we can move
        List<Space> spacesToGo = GetPossibleMovementSpots();
        //List<Space> spacesToGo = currentSpace.GetPossibleSpaces(player);
        keysSpaces.Clear();
        // == Clear old Arrow objects before making new ones == \\
        ui.RemoveOldArrows();

        //I'm unsure if the space that the player counts as going backwards needs to be seperate
        //for creating arrows and the like
        //visitedSpaces.Peek() will allow you to know which space the player just came from
        if (visitedSpaces.Count != 0)
        {
            //spacesToGo.Add(visitedSpaces.Peek());
        }

        foreach (Space space in spacesToGo)
        {
            //right now, forward is to the right
            Vector3 directionToSpace = space.transform.position - currentSpace.transform.position;

            //ui.SpawnArrowForSpace(currentSpace, space, directionToSpace);

            if (keyDirections.ContainsKey(directionToSpace))
            {
                SpaceDirection direction = keyDirections[directionToSpace];
                keysSpaces.Add(direction, space);
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
            ui.SpawnArrowForSpace(currentSpace, space);
        }
    }

    void CheckThenAddSpace(Vector3 vectorCheck, Space space)
    {
        SpaceDirection direction = keyDirections[vectorCheck];
        if (keysSpaces.ContainsKey(direction))
        {
            keysSpaces.Remove(direction);
        }
        else
        {
            keysSpaces.Add(direction, space);
        }
    }

    public List<Space> GetPossibleMovementSpots()
    {
        // return currentSpace.GetPossibleSpaces(player); gets the spaces that are considered 'forward'
        // return currentSpace.connectedSpaces.ToList(); gets all spaces that are connected to that space
        // this logic here gets all the spaces, to allow the user to go back, but keeps them from being able
        // to go backwards when they've moved from somewhere last turn
        List<Space> possibleSpaces = currentSpace.connectedSpaces.ToList();

        if (possibleSpaces.Contains(spaceLastTurnCameFrom))
        {
            possibleSpaces.Remove(spaceLastTurnCameFrom);
        }

        return possibleSpaces;
    }

    /// <summary>
    /// Logic to detemine where the player is now going to, if moving forward
    /// </summary>
    /// <param name="targetSpace"></param>
    public void MoveToSelectedSpace(Space targetSpace)
    {               
        //remove player from the previous space
        visitedSpaces.Push(currentSpace);
        currentSpace.LeaveSpace(player);

        currentSpace = MoveToSpace(targetSpace);
        spacesToMove--;      

        if (spacesToMove == 0)
        {
            player.StoppedMoving();
        }

        currentSpace.PlayerPassed(player);
    }

    public void ReverseMovement()
    {
        //if there isn't a previous space, don't go backwards
        if (visitedSpaces.Count == 0)
        {            
            return;
        }

        //let's grap the last space in visited spaces to go back to
        Space targetSpace = visitedSpaces.Pop();

        spacesToMove++;

        //undo the logic for passing the space
        currentSpace.LeaveSpace(player);
        currentSpace.PlayerReversed(player);

        //move the player to the target space
        currentSpace = MoveToSpace(targetSpace);

        //if we're back at the start
        if (visitedSpaces.Count == 0)
        {
            //reset movement logic back to where they were at the start of the turn
            //basically, forget that the player just came from the space they came from
            //and set it where they just came from the space they came from previously at the start of their turn
            targetSpace.LeaveSpace(player);
            targetSpace.EnterSpaceFrom(player, spaceLastTurnCameFrom);
        }
    }
    /// <summary>
    /// Modifiy any information here that needs to be finished up before the player's movements ends and before they move next turn
    /// </summary>
    public void EndMovement()
    {
        //it is in this order due to LandedUI
        currentSpace.PlayerLanded(player);

        //setup visited spaces for the next turn
        spaceLastTurnCameFrom = visitedSpaces.Pop();
        visitedSpaces.Clear();
    }

    public Space MoveToSpace(Space space)
    {
        //save the previous space as the space entered from
        space.EnterSpaceFrom(player, currentSpace);

        // === NEW CODE === \\
        StartCoroutine(LerpPosition(space.transform.position, (1 / moveSpeed)));

        return space;
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

    /// <summary>
    /// Needs a new name. Called when the player teleports or makes a decision at an intersection. Contains logic for moving and detecting if the 
    /// player has switched board sections. Whenever the player might have switched sections, it is better to call this than MoveToSpace()
    /// </summary>
    /// <param name="space"></param>
    /// <param name="hasTeleported"></param>
    public void TeleportToSpace(Space space, bool hasLanded)
    {
        currentSpace = space;

        if (hasLanded)
        {
            space.PlayerLanded(player);
        }

        player.transform.position = new Vector3(space.transform.position.x, heightOffset, space.transform.position.z);
        ClearSetDirection();
    }

    /// <summary>
    /// Allows the player to move either direction on their turn
    /// </summary>
    public void ClearSetDirection()
    {
        spaceLastTurnCameFrom = null;
    }
}

public enum SpaceDirection
{
    Right,
    Left,
    Up,
    Down,
}

