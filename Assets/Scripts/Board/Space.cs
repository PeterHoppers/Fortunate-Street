using UnityEngine;
public abstract class Space : MonoBehaviour
{
    public string Name { get; set; }

    public abstract void OnPlayerPass(Player player);
    public abstract void OnPlayerLand(Player player);
}