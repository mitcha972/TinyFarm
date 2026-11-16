using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlantItem : MonoBehaviour
{
    public Plant plant;

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

    public void BuyPlant()
    {
        Debug.Log("Selected " + plant.PlantName);
        fm.SelectPlant(this);
    }

    void InitializeUI()
    {
        nameTxt.text = plant.PlantName;
        priceTxt.text = "$" + plant.BuyPrice;
        icon.sprite = plant.Icon;
    }
}