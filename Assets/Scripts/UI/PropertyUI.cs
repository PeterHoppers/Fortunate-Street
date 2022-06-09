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
    }

    void OnDisable()
    {
        property.OnSpaceBought -= PropertyBought;
    }

    void PropertyBought(Player player, Space space)
    {
        buyHolder.SetActive(false);
        shopPriceHolder.SetActive(true);

        shopPrice.text = property.GetShopPrice().ToString();
        Debug.Log("Bought space: " + space);
    }
}
