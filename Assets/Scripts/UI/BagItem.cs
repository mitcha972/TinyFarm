using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BagItem : MonoBehaviour
{
    private Plant plant;

    //private FarmManager fm;

    [SerializeField] private Text nameTxt;
    [SerializeField] private Text priceTxt;
    [SerializeField] private Text quantityTxt;
    //public Text btnTxt;

    [SerializeField] private Image icon;

    private int quantity;     

    public void Setup(Plant plant, int amount)
    {
        this.plant = plant;
        nameTxt.text = plant.PlantName;
        //priceTxt.text = "$" + plant.SellPrice;
        icon.sprite = plant.Icon;

        SetAmount(amount);
    }
    
    public void SetAmount(int amount)
    {
        quantityTxt.text = "" + amount;
    }

}