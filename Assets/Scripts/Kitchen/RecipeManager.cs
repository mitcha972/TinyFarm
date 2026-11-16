using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeManager : MonoBehaviour
{
    public GameObject recipeItem;
    [SerializeField] private FarmManager fm;

   public List<RecipeObject> recipeObjects = new List<RecipeObject>();

    private void Awake()
    {
        //Assets/Resources/Recipes
        var loadRecipes = Resources.LoadAll("Recipes", typeof(RecipeObject));

        foreach (var recipe in loadRecipes){
            recipeObjects.Add((RecipeObject)recipe);
        }

        //recipeObjects.Sort(SortByPrice);

        foreach (var recipe in recipeObjects)
        {
            RecipeItem newRecipe = Instantiate(recipeItem, transform).GetComponent<RecipeItem>();
            newRecipe.orders = recipe.orders;
            newRecipe.recipeName = recipe.recipeName;
            newRecipe.bakeTime = recipe.bakeTime;
        }
    }
}