using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StallItem : MonoBehaviour
{
    public Plant plant;
    private FarmManager fm;
    private StallManager sm;
    public InputField inputField;

    public GameObject mm;
    public GameObject ci;

    public Text nameTxt;
    public Text priceTxt;
    public Text quantityTxt;
    public Text btnTxt;
    [SerializeField] private Text inputText;

    public Image icon;
    [SerializeField] public Sprite blankIcon;

    public int quantity;
    public int percent; 

    public bool isSettingUp;  
    public bool isActive;   

    void Awake()
    {
        fm = FindObjectOfType<FarmManager>();
        sm = FindObjectOfType<StallManager>();
        //inputText =  inputField.transform.GetChild(2).GetComponent<Text>();
        
        plant = null;
    }

    public void SellPlant()
    {
        Debug.Log("Sold 1 - " + plant.plantName);
        fm.RemovePlant(plant, 1);
        fm.Transaction(plant.sellPrice);
        quantity -= 1; 
        inputText.text = ""+ quantity;

    }

    public void ChoosePlant()
    {
        mm.SetActive(!mm.activeSelf);
        ci.SetActive(true);
        ci.GetComponent<ClickImage>().tab = this.gameObject;
        sm.selectedStall = this;
        this.gameObject.SetActive(false);
    }

    public void GetPlant(Plant getPlant)
    {
        if(plant == getPlant)
            return;

        plant = getPlant;
        icon.sprite = plant.icon;
        nameTxt.text = plant.plantName;
    }

    public void GetQuantity()
    {
        quantity = int.Parse(inputField.text);
        quantityTxt.text = "" + quantity;
    }

    public void GetPercent(int getPercent)
    {
        if(percent == getPercent)
            return;
        percent = getPercent;
        priceTxt.text = "$" + percent;
    }

    public void SetUpStall()
    {
        //Check plant is selected
        if(plant == null || quantity == 0)
        {
            Debug.Log("fields not full");
            return;
        }
        
        isActive = true;
        fm.RemovePlant(plant, quantity);
        btnTxt.text = "Close";
        inputField.gameObject.SetActive(false);
        inputText.text = ""+ quantity;
    }

    public void RemoveStall()
    {
        isActive = false;
        Debug.Log("Removed " + plant.plantName);
        plant = null;
        quantity = 0;
        percent = 0;
        quantityTxt.text = "-";
        priceTxt.text = "-";
        icon.sprite = blankIcon;
        nameTxt.text = "Crop..";
        btnTxt.text = "Start";
        inputText.text = "";
        inputField.gameObject.SetActive(true);
    
    }

    public void ButtonClick()
    {
        if(isActive)
            RemoveStall();
        else
           SetUpStall(); 
    } 
}