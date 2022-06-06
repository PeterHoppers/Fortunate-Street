using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Intersection : MonoBehaviour
{
    public Space[] connectedSpaces;

    Dictionary<Player, Space> spaceEnteredFrom = new Dictionary<Player, Space>();

    //remove the space the player entered from from the list of possible options the player can move to
    public List<Space> GetPossibleSpaces(Player player)
    {
        Space enteredSpace = spaceEnteredFrom[player];
        List<Space> possibleSpaces = connectedSpaces.ToList();

        if (enteredSpace == null)
        {
            return possibleSpaces;
        }

        possibleSpaces.Remove(enteredSpace);
        return possibleSpaces;        
    }

    /// <summary>
    /// This should be called whenever a player enters an intersection. That way, the space entered from is not returned as a possible place to move.
    /// </summary>
    /// <param name="player"></param>
    /// <param name="space"></param>
    public void EnterIntersection(Player player, Space space)
    {
        spaceEnteredFrom.Add(player, space);
    }

    public void LeaveIntersection(Player player)
    {
        spaceEnteredFrom.Remove(player);
    }    
}
