using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Recipe", menuName ="Recipe")]
public class RecipeData : ScriptableObject
{
    public string recipeName;
    public List<Order> orders;
    public float bakeTime;
    [SerializeField] private Plant outputPlant;
    public Plant OutputPlant => outputPlant;
    public Sprite icon;    
}