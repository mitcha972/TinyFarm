using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FarmManager : MonoBehaviour
{
    public PlantItem selectPlant;
    //public PlantFoodItem selectPlantFood;
    public MarketManager marketManager;
    public OrderManager orderManager;
    public BagManager bag;

    public int money = 100;
    public int level = 1;
    public int currentXp;

    [Header("Actions")]
    public bool isPlanting = false;
    public bool isBoosting = false;
    public bool isSelecting = false;
    public int selectedTool = 0; // 1- Water 2- Harvest 3- Buy plot

    public Color buyColor = Color.green;
    public Color cancelColor = Color.red;

    public Text moneyTxt;
    public Text levelTxt;
    public Image[] buttonsImg;
    public Sprite normalButton;
    public Sprite selectedButton;

    public List<Plant> plantInventory = new List<Plant>(); //Change to dictionary with plant and int quantity later
    //public Dictionary<Plant, int> plantInventory = new Dictionary<Plant, int>();

    void Start()
    {
        SetUpFarm();
    }

    private void SetUpFarm()
    {
        moneyTxt.text = "$" + money;
        currentXp = 0;
    }

    public void SelectPlant(PlantItem newPlant)
    {
        CheckSelection();
        if (selectPlant == newPlant)
        {
            selectPlant = null;
        }
        else
        {
            selectPlant = newPlant;
            selectPlant.btnImage.color = cancelColor;
            selectPlant.btnTxt.text = "Cancel";
            isPlanting = true;
        }
    }

    /*public void SelectPlantFood(PlantFoodItem newPlantFood)
    {
        CheckSelection();
        if (selectPlantFood == newPlantFood)
        {
            selectPlantFood = null;
        }
        else
        {
            selectPlantFood = newPlantFood;
            selectPlantFood.btnImage.color = cancelColor;
            selectPlantFood.btnTxt.text = "Cancel";
            isBoosting = true;
        }
    }*/

    public void SelectTool(int toolNumber)
    {
        if(toolNumber == selectedTool)
        {
            //Deselect
            CheckSelection();
        }
        else
        {
            //Select tool number and check to see if anything was also selected
            CheckSelection();
            isSelecting = true;
            selectedTool = toolNumber;
            buttonsImg[toolNumber - 1].sprite = selectedButton;
        }
    }

    void CheckSelection()
    {
        if (isPlanting)
        {
            isPlanting = false;
            if (selectPlant != null)
            {
                selectPlant.btnImage.color = buyColor;
                selectPlant.btnTxt.text = "Buy";
                selectPlant = null;
            }
        }

        if (isSelecting) //Selecting Tool
        {
            if (selectedTool > 0)
            {
                buttonsImg[selectedTool - 1].sprite = normalButton;
            }
            isSelecting = false;
            selectedTool = 0;
        }

    /*
        if (isBoosting)//Plant food
        {
            isBoosting = false;
            if (selectPlantFood != null)
            {
                selectPlantFood.btnImage.color = buyColor;
                selectPlantFood.btnTxt.text = "Buy";
                selectPlantFood = null;
            }
        }
    */        
    }

    public void Transaction(int value)
    {
        money += value;
        moneyTxt.text = "$" + money;
    }

    public void Experience(int value)
    {
        currentXp += value;

        if (currentXp >= 100)
            LevelUp();

        //UpdateXPBar();
    }

    private void LevelUp()
    {
        //TODO:
    }

    public void RemovePlant(Plant plant, int quantity)
    {
        for(int i = 0; i < plantInventory.Count; i++)
        {
            if(plantInventory[i].plantName == plant.plantName)
            {
                plantInventory[i].quantity -= quantity;
                marketManager.UpdateMarket(i, plantInventory[i].quantity);
                bag.UpdateMarket(i, plantInventory[i].quantity);

                if(plantInventory[i].quantity <= 0){
                    plantInventory.RemoveAt(i);
                    marketManager.RemoveFromMarket(plant.plantName);
                    bag.RemoveFromMarket(plant.plantName);
                }
            }
        }

        orderManager.CheckOrders();
    }

    public void AddPlant(Plant plant, int quantity)
    {
        if (plantInventory.Count != 0)
        {
            for(int i = 0; i < plantInventory.Count; i++)
            {
                if(plantInventory[i].plantName == plant.plantName)
                {
                    plantInventory[i].quantity += quantity;
                    marketManager.UpdateMarket(i,plantInventory[i].quantity);
                    bag.UpdateMarket(i,plantInventory[i].quantity);
                    orderManager.CheckOrders();
                    return;
                }
            }
                marketManager.AddToMarket(plant);
                bag.AddToMarket(plant);
                plantInventory.Add(plant);

        }else if (plantInventory.Count == 0)
        {
            marketManager.AddToMarket(plant);
            bag.AddToMarket(plant);
            plantInventory.Add(plant);     
        }
        
        orderManager.CheckOrders();
       
    } 
}

