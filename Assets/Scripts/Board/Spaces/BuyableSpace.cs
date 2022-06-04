public abstract class BuyableSpace : Space
{
    public Player owner;
    public District district;

    public int shopValue;

    public bool isOpen;

    public delegate void PlayerBuySpace(Player player, Space space);
    public event PlayerBuySpace OnSpaceBought;

    public virtual void SpaceBought(Player player)
    {
        if (OnSpaceBought != null)
        {
            OnSpaceBought(player, this);
        }         
    }
}
