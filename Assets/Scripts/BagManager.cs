using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagManager : MonoBehaviour
{
    [SerializeField] private BagItem bagItemPrefab;
    [SerializeField] private Transform contentArea;
    
    private readonly Dictionary<Plant, BagItem> items = new Dictionary<Plant, BagItem>();

    private void Start()
    {
        FarmManager.Instance.OnInventoryChanged += Refresh;
    }

    private void Refresh(Plant plant, int amount)
    {
        if (items.ContainsKey(plant))
        {
            if (amount <= 0)
            {
                Destroy(items[plant].gameObject);
                items.Remove(plant);
            }
            else
            {
                items[plant].SetAmount(amount);
            }
        }
        else
        {
            if (amount > 0)
            {
                BagItem newItem = Instantiate(bagItemPrefab, contentArea);
                newItem.Setup(plant, amount);
                items.Add(plant, newItem);
            }
        }
    }

    private void OnDestroy()
    {
        FarmManager.Instance.OnInventoryChanged -= Refresh;
    }
}