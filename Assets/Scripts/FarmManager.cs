using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;

public enum ToolType { None, Water, Harvest, BuyPlot }

public class FarmManager : MonoBehaviour
{
    [FormerlySerializedAs("selectPlant")] public PlantItem SelectedSeed;
    [SerializeField] private int money = 100;
    [SerializeField] private int level = 1;
    [SerializeField] private int currentXp;
    public int Money => money;
    public int Level => level;

    public bool isPlanting;
    public bool isSelecting;
    public ToolType selectedTool;
    public Color buyColor = Color.green;
    public Color cancelColor = Color.red;
    [SerializeField] private Text moneyTxt;
    [SerializeField] private Text levelTxt;
    [FormerlySerializedAs("buttonsImg")] [SerializeField] private Image[] toolButtonImages;
    [SerializeField] private Sprite normalButton;
    [SerializeField] private Sprite selectedButton;

    private readonly Dictionary<Plant, int> plantInventory = new Dictionary<Plant, int>();
    public IReadOnlyDictionary<Plant, int> Inventory { get; private set; }
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
        Inventory = new ReadOnlyDictionary<Plant, int>(plantInventory);
    }

    private void Start()
    {
        RefreshMoney();
        if (levelTxt != null) levelTxt.text = level.ToString();
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    public void SelectPlant(PlantItem newPlant)
    {
        bool clickedSame = SelectedSeed == newPlant;
        CheckSelection();
        if (clickedSame || newPlant == null) return;
        SelectedSeed = newPlant;
        SelectedSeed.SetSelected(true);
        isPlanting = true;
    }

    // Keep the integer entry point for existing Button OnClick arguments.
    public void SelectTool(int toolNumber)
    {
        if (toolNumber < (int)ToolType.None || toolNumber > (int)ToolType.BuyPlot) return;
        ToolType tool = (ToolType)toolNumber;
        bool clickedSame = selectedTool == tool;
        CheckSelection();
        if (clickedSame || tool == ToolType.None) return;
        isSelecting = true;
        selectedTool = tool;
        SetToolImage(tool, selectedButton);
    }

    private void SetToolImage(ToolType tool, Sprite sprite)
    {
        int index = (int)tool - 1;
        if (toolButtonImages != null && index >= 0 && index < toolButtonImages.Length && toolButtonImages[index] != null)
            toolButtonImages[index].sprite = sprite;
    }

    private void CheckSelection()
    {
        if (SelectedSeed != null)
        {
            SelectedSeed.SetSelected(false);
        }
        SelectedSeed = null;
        isPlanting = false;
        SetToolImage(selectedTool, normalButton);
        isSelecting = false;
        selectedTool = ToolType.None;
    }

    public bool TrySpend(int amount)
    {
        if (amount < 0 || amount > money) return false;
        money -= amount;
        RefreshMoney();
        return true;
    }

    public void AddMoney(int amount)
    {
        if (amount <= 0 || amount > int.MaxValue - money) return;
        money += amount;
        RefreshMoney();
    }

    // Preserve existing Inspector callbacks while using the checked operations.
    public void Transaction(int value)
    {
        if (value >= 0) AddMoney(value);
        else if (value != int.MinValue) TrySpend(-value);
    }

    private void RefreshMoney()
    {
        if (moneyTxt != null) moneyTxt.text = "$" + money;
    }

    public void Experience(int value)
    {
        if (value > 0) currentXp += value;
        // Level progression is unfinished; keep its existing behaviour.
    }

    public int GetAmount(Plant plant)
    {
        return plant != null && plantInventory.TryGetValue(plant, out int amount) ? amount : 0;
    }

    public bool TryRemovePlant(Plant plant, int amount)
    {
        if (plant == null || amount <= 0 || GetAmount(plant) < amount) return false;
        int remaining = GetAmount(plant) - amount;
        if (remaining == 0) plantInventory.Remove(plant);
        else plantInventory[plant] = remaining;
        OnInventoryChanged?.Invoke(plant, remaining);
        return true;
    }

    public bool TrySellPlant(Plant plant, int amount)
    {
        if (plant == null || amount <= 0 || plant.SellPrice < 0) return false;
        long proceeds = (long)plant.SellPrice * amount;
        if (proceeds > int.MaxValue - money || !TryRemovePlant(plant, amount)) return false;
        AddMoney((int)proceeds);
        return true;
    }

    public void RemovePlant(Plant plant, int amount)
    {
        TryRemovePlant(plant, amount);
    }

    public void AddPlant(Plant plant, int amount)
    {
        if (plant == null || amount <= 0 || amount > int.MaxValue - GetAmount(plant)) return;
        int total = GetAmount(plant) + amount;
        plantInventory[plant] = total;
        OnInventoryChanged?.Invoke(plant, total);
    }
}
