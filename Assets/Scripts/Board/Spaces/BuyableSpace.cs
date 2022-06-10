public abstract class BuyableSpace : Space
{
    public Player owner;

    public int shopValue;

    public bool isOpen;

    public delegate void PlayerBuySpace(Player player, BuyableSpace space);
    public event PlayerBuySpace OnSpaceBought;

    public delegate void PlayerSpaceValueChange(BuyableSpace space);
    public event PlayerSpaceValueChange OnSpaceValueChanged;

    public virtual void SpaceBought(Player player)
    {
        owner = player;
        OnSpaceBought?.Invoke(player, this);
    }

    public virtual void ValueChanged()
    {
        OnSpaceValueChanged?.Invoke(this);
    }

    /// <summary>
    /// Changes the shop value based upon the the amount and the orginal shop value
    /// To double the price, amountChange would be 2
    /// This one is abstract, since we don't want to call this method itself, only its implementation
    /// </summary>
    /// <param name="amountChange"></param>
    public abstract void ChangeShopValue(double amountChange);
}
