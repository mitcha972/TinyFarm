using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarketManager : MonoBehaviour
{
    public GameObject marketItem;
    [SerializeField] private FarmManager fm;
    [SerializeField] private StallManager sm;
    [SerializeField] private MarketManager mm;

    List<MarketItem> marketItems = new List<MarketItem>();

    [SerializeField] private Transform contentArea;

    private void Awake()
    {
        /*
        foreach (Plant plant in fm.plantInventory)
        {
            MarketItem newPlant = Instantiate(marketItem, contentArea).GetComponent<MarketItem>();
            newPlant.plant = plant;
            marketItems.Add(newPlant);
        }
        */
    }

    public void AddToMarket(Plant plant)
    {
        MarketItem newPlant = Instantiate(marketItem, contentArea).GetComponent<MarketItem>();
        newPlant.plant = plant;
        newPlant.mm = mm;
        newPlant.sm = sm;
        marketItems.Add(newPlant);
    }

    public void RemoveFromMarket(string plantName)
    {
        for(int i = 0; i< marketItems.Count; i++)
            if(marketItems[i].plant.PlantName == plantName){
                MarketItem I = marketItems[i];
                marketItems.Remove(I);
                Destroy(I.gameObject);
            }
    }

    public void UpdateMarket(int i, int quant)
    {
        marketItems[i].quantity = quant;
        marketItems[i].quantityTxt.text = "" + quant;
    }
}