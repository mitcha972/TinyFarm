using System.Collections.Generic;
using UnityEngine;

public class MarketManager : MonoBehaviour
{
    [SerializeField] private GameObject marketItem;
    [SerializeField] private StallManager sm;
    [SerializeField] private Transform contentArea;
    private readonly List<MarketItem> marketItems = new List<MarketItem>();

    private void Start()
    {
        FarmManager.Instance.OnInventoryChanged += Refresh;
        foreach (var entry in FarmManager.Instance.Inventory)
            Refresh(entry.Key, entry.Value);
    }

    private void OnDestroy()
    {
        if (FarmManager.Instance != null)
            FarmManager.Instance.OnInventoryChanged -= Refresh;
    }

    private void Refresh(Plant plant, int amount)
    {
        for (int i = marketItems.Count - 1; i >= 0; i--)
        {
            MarketItem item = marketItems[i];
            if (item.plant != plant) continue;
            if (amount > 0) item.SetAmount(amount);
            else
            {
                marketItems.RemoveAt(i);
                Destroy(item.gameObject);
            }
            return;
        }
        if (amount > 0) AddToMarket(plant);
    }

    public void AddToMarket(Plant plant)
    {
        if (plant == null || marketItems.Exists(item => item.plant == plant)) return;
        MarketItem item = Instantiate(marketItem, contentArea).GetComponent<MarketItem>();
        item.Setup(plant, this, sm);
        marketItems.Add(item);
    }

    public void RemoveFromMarket(string plantName)
    {
        for (int i = marketItems.Count - 1; i >= 0; i--)
        {
            MarketItem item = marketItems[i];
            if (item.plant.PlantName != plantName) continue;
            marketItems.RemoveAt(i);
            Destroy(item.gameObject);
        }
    }

    public void UpdateMarket(int index, int amount)
    {
        if (index >= 0 && index < marketItems.Count) marketItems[index].SetAmount(amount);
    }
}
