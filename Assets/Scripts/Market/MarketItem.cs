using UnityEngine;
using UnityEngine.UI;

public class MarketItem : MonoBehaviour
{
    public Plant plant;
    private StallManager stallManager;
    private MarketManager marketManager;
    [SerializeField] private Text nameTxt;
    [SerializeField] private Text priceTxt;
    [SerializeField] private Text quantityTxt;
    [SerializeField] private Image icon;
    [SerializeField] private int quantity;

    public void Setup(Plant plant, MarketManager marketManager, StallManager stallManager)
    {
        this.plant = plant;
        this.marketManager = marketManager;
        this.stallManager = stallManager;
        nameTxt.text = plant.PlantName;
        priceTxt.text = "$" + plant.SellPrice;
        icon.sprite = plant.Icon;
        SetAmount(FarmManager.Instance.GetAmount(plant));
    }

    public void SetAmount(int amount)
    {
        quantity = amount;
        quantityTxt.text = amount.ToString();
    }

    public void SelectPlant()
    {
        if (stallManager == null || stallManager.selectedStall == null) return;
        stallManager.selectedStall.GetPlant(plant);
        CloseSelection();
    }

    public void CloseSelection()
    {
        if (stallManager == null || stallManager.selectedStall == null) return;
        stallManager.selectedStall.gameObject.SetActive(true);
        stallManager.selectedStall.contentArea.SetActive(false);
        stallManager.selectedStall = null;
        marketManager.gameObject.SetActive(false);
    }

    public void SellPlant()
    {
        FarmManager.Instance.TrySellPlant(plant, 1);
    }

    public void SellAllPlants()
    {
        FarmManager farm = FarmManager.Instance;
        farm.TrySellPlant(plant, farm.GetAmount(plant));
    }
}
