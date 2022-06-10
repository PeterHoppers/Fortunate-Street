using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PropertyUI : MonoBehaviour
{
    public TMP_Text buyProperty;
    public TMP_Text shopPrice;

    GameObject buyHolder;
    GameObject shopPriceHolder;
    Property property;

    // Start is called before the first frame update
    void Awake()
    {
        property = GetComponent<Property>();

        //right now, the values of buyHolders and shopPrice holders are hard coded. This might all change when we change how they look
        buyHolder = buyProperty.transform.parent.parent.gameObject;
        shopPriceHolder = shopPrice.transform.parent.gameObject;

        buyProperty.text = property.shopValue.ToString();

        buyHolder.SetActive(true);
        shopPriceHolder.SetActive(false);
    }

    void OnEnable()
    {
        property.OnSpaceBought += PropertyBought;
        property.OnSpaceValueChanged += ValueChanged;
    }

    void OnDisable()
    {
        property.OnSpaceBought -= PropertyBought;
        property.OnSpaceValueChanged -= ValueChanged;
    }

    /// <summary>
    /// Update UI to reflect the owner of the space
    /// </summary>
    /// <param name="player"></param>
    /// <param name="space"></param>
    void PropertyBought(Player player, Space space)
    {
        buyHolder.SetActive(false);
        shopPriceHolder.SetActive(true);

        shopPrice.text = property.GetShopPrice().ToString();
    }

    void ValueChanged(BuyableSpace space)
    {
        shopPrice.text = property.GetShopPrice().ToString();
    }
}
