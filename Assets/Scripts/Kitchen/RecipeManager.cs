using System.Collections.Generic;
using UnityEngine;

public class RecipeManager : MonoBehaviour
{
    [SerializeField] private GameObject recipeItem;
    [SerializeField] private Transform contentArea;
    [SerializeField] private List<RecipeData> recipeObjects = new List<RecipeData>();

    private void Awake()
    {
        recipeObjects.Clear();
        recipeObjects.AddRange(Resources.LoadAll<RecipeData>("Recipes"));
    }

    private void Start()
    {
        foreach (RecipeData recipe in recipeObjects)
        {
            RecipeItem item = Instantiate(recipeItem, contentArea).GetComponent<RecipeItem>();
            item.Setup(recipe);
        }
    }
}
