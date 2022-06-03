using UnityEngine;
public abstract class Space : MonoBehaviour
{
    public string Name { get; set; }

    public abstract void OnPlayerLand(Player player);
    public abstract void OnPlayerPass(Player player);
    
}