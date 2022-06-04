using UnityEngine;
public class Space : MonoBehaviour
{
    public string spaceName;

    //These are events that the base class handles calling so anything can subscribe to a space and get this information
    public delegate void PlayerPass(Player player, Space space);
    public static event PlayerPass OnPlayerPass;
    public delegate void PlayerLand(Player player, Space space);
    public static event PlayerLand OnPlayerLand;

    public virtual void PlayerPassed(Player player)
    {
        OnPlayerPass?.Invoke(player, this);
    }
    public virtual void PlayerLanded(Player player)
    {
        //short hand for if(OnPlayerLand != null) OnPlayerLand(player, space)
        OnPlayerLand?.Invoke(player, this);
    }    
}