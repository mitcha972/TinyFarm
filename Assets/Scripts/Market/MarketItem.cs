using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarketItem : MonoBehaviour
{
    public Plant plant;

    private FarmManager fm;
    public StallManager sm;
    public MarketManager mm;
    public StallItem stallItem;

    public Text nameTxt;
    public Text priceTxt;
    public Text quantityTxt;
    //public Text btnTxt;

    public Image icon;

    public int quantity;     

    void Awake()
    {
        fm = FindObjectOfType<FarmManager>();

        InitializeUI();
    }

    public void SelectPlant()
    {
        if(sm.selectedStall == null)
        {
            Debug.Log("noStall");
            return;
        }

        sm.selectedStall.GetPlant(plant);
        CloseSelection();
    }

    public void CloseSelection()
    {
        sm.selectedStall.gameObject.SetActive(true);
        sm.selectedStall.ci.SetActive(false);
        mm.gameObject.SetActive(false);
    }

    public void SellPlant()
    {
        Debug.Log("Sold 1 - " + plant.plantName);
        fm.RemovePlant(plant, 1);
        fm.Transaction(plant.sellPrice);
    }

    public void SellAllPlants()
    {
        Debug.Log("Sold All " + plant.plantName);
        fm.Transaction(plant.sellPrice * plant.quantity);
        fm.RemovePlant(plant, plant.quantity);
        
    }

    void InitializeUI()
    {
        nameTxt.text = plant.plantName;
        priceTxt.text = "$" + plant.sellPrice;
        quantityTxt.text = "" + plant.quantity;
        icon.sprite = plant.icon;
    }
}