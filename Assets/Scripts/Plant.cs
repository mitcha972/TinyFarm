using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Plant", menuName ="Plant")]
public class Plant : ScriptableObject
{
    public string plantName;
    public int sellPrice;
    public int buyPrice;
    public int quantity;
    public Sprite[] plantStages;
    public float timeBtwStages;
    public int unlockLevel;
    public Sprite icon;
    public Sprite dryPlanted;
    
    public Plant(string pName, int sPrice, Sprite con)
    {
        plantName = pName;
        sellPrice = sPrice;
        icon = con;
        quantity = 1;
    }
}