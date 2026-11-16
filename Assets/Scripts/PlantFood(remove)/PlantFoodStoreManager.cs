using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantFoodStoreManager : MonoBehaviour
{
    public GameObject plantFoodItem;
    List<PlantFoodObject> plantFoodObjects = new List<PlantFoodObject>();

    private void Awake()
    {
        //Assets/Resources/PlantFoods
        var loadPlantFoods = Resources.LoadAll("PlantFoods", typeof(PlantFoodObject));

        foreach (var plantFood in loadPlantFoods){
            plantFoodObjects.Add((PlantFoodObject)plantFood);
        }

        plantFoodObjects.Sort(SortByPrice);

        foreach (var plantFood in plantFoodObjects)
        {
            PlantFoodItem newPlantFood = Instantiate(plantFoodItem, transform).GetComponent<PlantFoodItem>();
            newPlantFood.plantFood = plantFood;
        }
    }

    int SortByPrice(PlantFoodObject plantFoodObject1, PlantFoodObject plantFoodObject2)
    {
        return plantFoodObject1.buyPrice.CompareTo(plantFoodObject2.buyPrice);
    }
}