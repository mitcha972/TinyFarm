using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarketItem : MonoBehaviour
{
    public Plant plant;

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
        Debug.Log("Sold 1 - " + plant.PlantName);
        FarmManager.Instance.RemovePlant(plant, 1);
        FarmManager.Instance.Transaction(plant.SellPrice);
    }

    public void SellAllPlants()
    {
        Debug.Log("Sold All " + plant.PlantName);
        FarmManager.Instance.Transaction(plant.SellPrice * FarmManager.Instance.GetAmount(plant));
        FarmManager.Instance.RemovePlant(plant, FarmManager.Instance.GetAmount(plant));
        
    }

    void InitializeUI()
    {
        nameTxt.text = plant.PlantName;
        priceTxt.text = "$" + plant.SellPrice;
        quantityTxt.text = "" + FarmManager.Instance.GetAmount(plant);
        icon.sprite = plant.Icon;
    }
}