using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BagItem : MonoBehaviour
{
    public Plant plant;

    private FarmManager fm;

    public Text nameTxt;
    public Text priceTxt;
    public Text quantityTxt;
    //public Text btnTxt;

    public Image icon;

    public int quantity;     

    void Start()
    {
        fm = FindObjectOfType<FarmManager>();
        InitializeUI();
    }

    void InitializeUI()
    {
        //nameTxt.text = plant.plantName;
        //priceTxt.text = "$" + plant.sellPrice;
        quantityTxt.text = "" + plant.quantity;
        icon.sprite = plant.icon;
    }
}