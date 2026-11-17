using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlantFoodItem : MonoBehaviour
{
    public PlantFoodObject plantFood;



    public Text nameTxt;
    public Text priceTxt;
    public Text btnTxt;

    public Image icon;
    public Image btnImage;      

    void Start()
    {

        InitializeUI();
    }

    public void BuyPlantFood()
    {
        Debug.Log("Bought " + plantFood.plantFoodName);
    }

    void InitializeUI()
    {
        nameTxt.text = plantFood.plantFoodName;
        priceTxt.text = "$" + plantFood.buyPrice;
        icon.sprite = plantFood.icon;
    }

}