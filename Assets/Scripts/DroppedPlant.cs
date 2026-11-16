using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DroppedPlant : MonoBehaviour
{
    public Plant plant;
    public Image icon;
    private float timer;
    private FarmManager fm;

    void Start()
    {
        timer = 0;
        InitializeUI();
    }

    void InitializeUI()
    {
        icon.sprite = plant.icon;
    }

    private void OnMouseOver()
    {
        CollectPlant();
    }

    private void CollectPlant()
    {
        fm.AddPlant(plant, 1);
        Destroy(this.gameObject);
    }
}
