public abstract class Space 
{
    public string Name { get; set; }

    public abstract void OnPlayerPass();
    public abstract void OnPlayerLand();
}