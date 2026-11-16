using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantManager : MonoBehaviour
{
    public GameObject plantItem;
    public FarmManager fm;

    public List<Plant> plantObjects = new List<Plant>();

    private void Awake()
    {
        //Assets/Resources/Plants
        var loadPlants = Resources.LoadAll("Plants", typeof(Plant));

        foreach (var plant in loadPlants){
            plantObjects.Add((Plant)plant);
        }

        plantObjects.Sort(SortByPrice);

        foreach (var plant in plantObjects)
        {
            PlantItem newPlant = Instantiate(plantItem, transform).GetComponent<PlantItem>();
            newPlant.plant = plant;

            //Check Plant Unlock level
            if (plant.unlockLevel <= fm.level)
                newPlant.gameObject.SetActive(true);
        }
    }

    public void UnlockPlant()
    {
        
    }

    int SortByPrice(Plant plantObject1, Plant plantObject2)
    {
        return plantObject1.buyPrice.CompareTo(plantObject2.buyPrice);
    }

    int SortByTime(Plant plantObject1, Plant plantObject2)
    {
        return plantObject1.timeBtwStages.CompareTo(plantObject2.timeBtwStages);
    }
}