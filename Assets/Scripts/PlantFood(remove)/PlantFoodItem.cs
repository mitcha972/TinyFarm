using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlantFoodItem : MonoBehaviour
{
    public PlantFoodObject plantFood;

    private FarmManager fm;

    public Text nameTxt;
    public Text priceTxt;
    public Text btnTxt;

    public Image icon;
    public Image btnImage;      

    void Start()
    {
        fm = FindObjectOfType<FarmManager>();
        InitializeUI();
    }

    public void BuyPlantFood()
    {
        Debug.Log("Bought " + plantFood.plantFoodName);
       // fm.SelectPlantFood(this);
    }

    void InitializeUI()
    {
        nameTxt.text = plantFood.plantFoodName;
        priceTxt.text = "$" + plantFood.buyPrice;
        icon.sprite = plantFood.icon;
    }

}