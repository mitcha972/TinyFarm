using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Recipe", menuName ="Recipe")]
public class RecipeObject : ScriptableObject
{
    public string recipeName;
    public List<Order> orders;
    public float bakeTime;
    public Sprite icon;    
}