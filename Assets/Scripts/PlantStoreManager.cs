using System.Collections.Generic;
using UnityEngine;

public class PlantStoreManager : MonoBehaviour
{
    [SerializeField] private GameObject plantItem;
    public List<Plant> plantObjects = new List<Plant>();
    [SerializeField] private Transform contentArea;

    private void Awake()
    {
        plantObjects.Clear();
        plantObjects.AddRange(Resources.LoadAll<Plant>("Plants"));
        plantObjects.Sort((first, second) => first.BuyPrice.CompareTo(second.BuyPrice));
    }

    private void Start()
    {
        foreach (Plant plant in plantObjects)
        {
            if (plant.UnlockLevel > FarmManager.Instance.Level) continue;
            PlantItem item = Instantiate(plantItem, contentArea).GetComponent<PlantItem>();
            item.Setup(plant);
            item.gameObject.SetActive(true);
        }
    }
}
