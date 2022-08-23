using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Modal that appears that contains the information about the property allowing a player to purchase it.
/// </summary>
public class PurchasePropertyUI : MonoBehaviour
{
    Player playerPurchaser;
    Property targetProperty;

    /// <summary>
    /// Setups up the UI to reflect the property and player purchasing it
    /// </summary>
    /// <param name="player"></param>
    /// <param name="property"></param>
    public void SetupUI(Player player, Property property)
    {
        playerPurchaser = player;
        targetProperty = property;
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Called from the UIManager. Seperate from buy/decline buying the property due to 
    /// AI and possibly allowing a user to back out from the property they want to choose to buy
    /// say from an event
    /// </summary>
    public void HideUI()
    {
        gameObject.SetActive(false);
    }

    public void BuySpace()
    {
        targetProperty.BuySpace(playerPurchaser);
    }

    public void DeclineSpace()
    {
        targetProperty.DeclineBuySpace(playerPurchaser);
    }

}
