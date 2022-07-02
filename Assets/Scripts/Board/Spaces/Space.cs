using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Space : MonoBehaviour
{
    public string spaceName;
    public Space[] connectedSpaces;

    Dictionary<Player, Space> spaceEnteredFrom = new Dictionary<Player, Space>();

    //These are events that the base class handles calling so anything can subscribe to a space and get this information
    public delegate void PlayerPass(Player player, Space space);
    public static event PlayerPass OnPlayerPass;
    public delegate void PlayerLand(Player player, Space space);
    public static event PlayerLand OnPlayerLand;
    public delegate void PlayerReverse(Player player, Space space);
    public static event PlayerReverse OnPlayerReverse;

    /// <summary>
    /// What happens when the player just happens to pass by a space
    /// </summary>
    /// <param name="player"></param>
    public virtual void PlayerPassed(Player player)
    {
        OnPlayerPass?.Invoke(player, this);
    }
    /// <summary>
    /// What happens when the player decides to land on that space. If it also occurs when the player passes, only add it to player passes
    /// </summary>
    /// <param name="player"></param>
    public virtual void PlayerLanded(Player player)
    {
        //short hand for if(OnPlayerLand != null) OnPlayerLand(player, space)
        OnPlayerLand?.Invoke(player, this);
        player.SetTurnState(TurnState.Landed);
    }
    /// <summary>
    /// Right now, the way we undo the logic that occurs in player passes
    /// </summary>
    /// <param name="player"></param>
    public virtual void PlayerReversed(Player player)
    {
        OnPlayerReverse?.Invoke(player, this);
    }

    //remove the space the player entered from from the list of possible options the player can move to
    public List<Space> GetPossibleSpaces(Player player)
    {
        List<Space> possibleSpaces = connectedSpaces.ToList();

        //if on start, teleported, or option to go both ways is activated
        if (!spaceEnteredFrom.ContainsKey(player))
        {
            Debug.Log($"No record of the player entering this space from anywhere.");
            return possibleSpaces;
        }

        //Debug.Log($"The player entered from {spaceEnteredFrom[player]}");
        Space enteredSpace = spaceEnteredFrom[player];
        possibleSpaces.Remove(enteredSpace);
        return possibleSpaces;
    }

    /// <summary>
    /// This should be called whenever a player enters an intersection. That way, the space entered from is not returned as a possible place to move.
    /// </summary>
    /// <param name="player"></param>
    /// <param name="space"></param>
    public void EnterSpaceFrom(Player player, Space space)
    {
        spaceEnteredFrom.Add(player, space);
    }

    public bool HasPlayerEntered(Player player)
    {
        return spaceEnteredFrom.ContainsKey(player);
    }

    public Space WherePlayerEnteredFrom(Player player)
    {
        if (!spaceEnteredFrom.ContainsKey(player))
        {
            return null;
        }
        return spaceEnteredFrom[player];
    }


    public void LeaveSpace(Player player)
    {
        spaceEnteredFrom.Remove(player);
    }
}