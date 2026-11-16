using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagManager : MonoBehaviour
{
    public GameObject bagItem;
    [SerializeField] private FarmManager fm;

    private List<BagItem> bagItems = new List<BagItem>();

    private void Awake()
    {
        /*
        foreach (Plant plant in fm.plantInventory)
        {
            BagItem newPlant = Instantiate(bagItem, transform).GetComponent<BagItem>();
            newPlant.plant = plant;
            bagItems.Add(newPlant);
        }
        */
    }

    private void PopulateBag()
    {
        
    }

    public void AddToMarket(Plant plant)
    {
        BagItem newPlant = Instantiate(bagItem, transform).GetComponent<BagItem>();
        newPlant.plant = plant;
        bagItems.Add(newPlant);
    }

    public void RemoveFromMarket(string plantName)
    {
        for(int i = 0; i< bagItems.Count; i++)
            if(bagItems[i].plant.plantName == plantName){
                BagItem I = bagItems[i];
                bagItems.Remove(I);
                Destroy(I.gameObject);
            }
    }

    public void UpdateMarket(int i, int quant)
    {
        bagItems[i].quantity = quant;
        bagItems[i].quantityTxt.text = "" + quant;
    }
}