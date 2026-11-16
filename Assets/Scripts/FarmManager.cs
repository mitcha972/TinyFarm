using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class FarmManager : MonoBehaviour
{
    public PlantItem selectPlant;
    //public PlantFoodItem selectPlantFood;
    public MarketManager marketManager;
    public OrderManager orderManager;

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

    //public List<Plant> plantInventory = new List<Plant>(); //Change to dictionary with plant and int quantity later
    public Dictionary<Plant, int> plantInventory = new Dictionary<Plant, int>();

    public event Action<Plant, int> OnInventoryChanged;

    public static FarmManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

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

    public int GetAmount(Plant plant)
    {
        return plantInventory.TryGetValue(plant, out int amount) ? amount : 0;
    }

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

    public void RemovePlant(Plant plant, int amount)
    {
        if (amount <= 0) return;

        int newAmount = Mathf.Max(0, GetAmount(plant) - amount);

        if (newAmount == 0)
            plantInventory.Remove(plant);
        else
            plantInventory[plant] = newAmount;

        OnInventoryChanged?.Invoke(plant, newAmount);
        //orderManager.CheckOrders();
    }

    public void AddPlant(Plant plant, int amount)
    {
        if (amount <= 0) return;

        int newAmount = GetAmount(plant) + amount;
        plantInventory[plant] = newAmount;

        OnInventoryChanged?.Invoke(plant, newAmount);
        //orderManager.CheckOrders();
    }
}

