using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName ="New Plant Food", menuName ="PlantFood")]
public class PlantFoodObject : ScriptableObject
{
    public string plantFoodName;
    public float growSpeed;
    public int buyPrice;
    public Sprite icon;
    //public Color fColor;
    
}
