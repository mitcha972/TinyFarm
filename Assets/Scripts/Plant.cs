using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Plant", menuName ="Plant")]
public class Plant : ScriptableObject
{
    [SerializeField] private string plantName;
    public string PlantName => plantName;
    [SerializeField] private int sellPrice;
    public int SellPrice => sellPrice;

    [SerializeField] private int buyPrice;
    public int BuyPrice => buyPrice;
    
    [SerializeField] private Sprite[] plantStages;
    public Sprite[] PlantStages => plantStages;

    [SerializeField] private float timeBtwStages;
    public float TimeBtwStages => timeBtwStages;
    [SerializeField] private int unlockLevel;
    public int UnlockLevel => unlockLevel;
    [SerializeField] private Sprite icon;
    public Sprite Icon => icon;
    [SerializeField] private Sprite dryPlanted;
    public Sprite DryPlanted => dryPlanted;
}