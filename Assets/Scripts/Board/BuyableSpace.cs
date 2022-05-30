public abstract class BuyableSpace : Space
{
    public Player owner;
    public District district;
    public double shopValue;

    public abstract void OnSpaceBought();
}
