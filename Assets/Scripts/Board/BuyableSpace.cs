public abstract class BuyableSpace : Space
{
    public Player owner;
    public District district;

    public bool isOpen;

    public abstract void OnSpaceBought(Player player);
}
