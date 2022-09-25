using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StockDistrictUI : MonoBehaviour
{
    public TMP_Text district;
    public TMP_Text amount;
    public TMP_Text stocksOwned;

    [HideInInspector]
    public District referencedDistrict;

    public void SetupDisplay(string districtName, int price, int playerOwned)
    {
        district.text = "District: " + districtName;
        amount.text =  "Price: " + price.ToString();
        stocksOwned.text = "Player Owned: " + playerOwned.ToString();
    }
}
