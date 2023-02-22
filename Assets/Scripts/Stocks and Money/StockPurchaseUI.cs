using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class StockPurchaseUI : MonoBehaviour
{    
    public GameObject stockSelect;
    public StockDistrictUI stockDistrict;
    public DialReader dialReader;
    public TMP_Text spendingText;
    public Button purchaseBtn;

    Player stockPurchaser;
    District selectedStock;

    void OnEnable()
    {
        dialReader.OnDialValueChange += DisplaySpending;
    }

    void OnDisable()
    {
        dialReader.OnDialValueChange -= DisplaySpending;
    }

    public void SetupUI(Player player)
    {
        stockPurchaser = player;
        gameObject.SetActive(true);
        stockPurchaser.PauseMoving();
        purchaseBtn.Select();

        District[] districts = BoardManager.districts;

        //if the UI has been fully created, create it
        if (stockSelect.transform.childCount == 0)
        {
            foreach (District district in districts)
            {
                StockDistrictUI newStockUI = Instantiate(stockDistrict, stockSelect.transform);
                UpdateDistrictUI(newStockUI, stockPurchaser, district);
                newStockUI.referencedDistrict = district;
                newStockUI.GetComponent<Button>().onClick.AddListener(SelectStock);
            }
        }
        else //we update the UI based upon the new information
        { 
            StockDistrictUI[] stockDistrictUIs = stockSelect.GetComponentsInChildren<StockDistrictUI>();
            foreach (StockDistrictUI stockUI in stockDistrictUIs)
            {
                UpdateDistrictUI(stockUI, stockPurchaser, stockUI.referencedDistrict);
            }
        }        
    }

    void UpdateDistrictUI(StockDistrictUI stockUI, Player player, District district)
    {
        stockUI.SetupDisplay(
            district.Name,
            district.StockPrice,
            StockManager.GetPlayerStockInDistrict(player, district)
        );
    }

    public void SelectStock()
    {
        GameObject clickedStock = EventSystem.current.currentSelectedGameObject;

        StockDistrictUI clickedUI = clickedStock.GetComponent<StockDistrictUI>();

        if (clickedUI != null)
        {
            selectedStock = clickedUI.referencedDistrict;
            DisplaySpending();
        }
    }

    public void DisplaySpending()
    {
        DisplaySpending(dialReader.GetValue());
    }

    public void DisplaySpending(int dialValue)
    {
        if (selectedStock == null)
        {
            return;
        }

        int possibleSpending = dialValue * selectedStock.StockPrice;

        spendingText.text = possibleSpending.ToString();
    }

    public void PurchaseStocks()
    {
        if (selectedStock == null)
        {
            Debug.LogError("A stock needs to be chosen first!");
            return;
        }

        int stocksChosen = dialReader.GetValue();

        if (!StockManager.CanBuyStock(stockPurchaser, selectedStock, stocksChosen))
        {
            Debug.LogError($"{stockPurchaser} cannot buy {stocksChosen} of {selectedStock}'s stock");
            return;
        }

        StockManager.BuyStock(stockPurchaser, selectedStock, stocksChosen);
        gameObject.SetActive(false);
        stockPurchaser.UnPauseMoving();
    }
}
