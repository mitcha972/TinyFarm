using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeItem : MonoBehaviour
{
    private FarmManager fm;
    [SerializeField] private GameObject recipeObj;
    [SerializeField] private Transform HGL;
    public List<Order> orders;

    public Text nameTxt;
    public Text priceTxt;
    public Text btnTxt;

    public Button btn;

    public Image bkg;

    private int price; 
    public int precent;

    public float bakeTime;
    private float timer;
    private float speed = 1f; 

    public string recipeName;

    public bool isBaking = false;

    void Start()
    {
        timer = bakeTime;
        fm = FindObjectOfType<FarmManager>();
        InitializeUI();
    }

    private void Update()
    {
        if (isBaking)
        {
            timer -= speed*Time.deltaTime;

            if (timer < 0){
                Bake();
            }       
        }
    }

    public void CheckRecipe()
    {  
        for(int i = 0; i < orders.Count; i++)
        {      
            if(PlantQuantity(orders[i].plant) < orders[i].quantity)
            {
                Debug.Log("noFire");
                return;                
            }
        }

        btnTxt.text = "Baking";
        var tempColor = bkg.color;
        tempColor.a = 0.15f;  
        bkg.color = tempColor;  
        btn.interactable = false;  
        RemovePlants();        
    }

    private int PlantQuantity(Plant plant)
    {
        for(int i = 0; i < fm.plantInventory.Count; i++){
            if(fm.plantInventory[i].plantName == plant.plantName)
                return fm.plantInventory[i].quantity;
        }

        return 0;
    }   

    public void RemovePlants()
    { 
       for(int i = 0; i < orders.Count; i++)
        {
            //Debug.Log("Sold " + orders[i].quantity + " - "+ orders[i].plant.plantName);
            fm.RemovePlant(orders[i].plant, orders[i].quantity);
        }  

        isBaking = true;    
    }

    public void Bake()
    { 
        isBaking = false;
        timer = bakeTime;
        btnTxt.text = "Bake";
        var tempColor = bkg.color;
        tempColor.a = 0f;  
        bkg.color = tempColor;
        btn.interactable = true;

        fm.AddPlant(new Plant(recipeName,price,orders[0].plant.icon),1);
    }

    void InitializeUI()
    {
        //Transform HGL = transform.Find("HGL");

        for(int i = 0; i < orders.Count; i++)
        {
            //Add order objects 
            //RecipeObj newR = Instantiate(recipeObj, transform).GetComponent<RecipeObj>();
            //newR.transform.SetParent(HGL);
            HGL.GetChild(i).gameObject.GetComponent<RecipeObj>().InitializeUI(orders[i].quantity,orders[i].plant.icon);
            //newR.InitializeUI(orders[i].quantity,orders[i].plant.icon);
            
            //Initialize New Recipe
            price += (orders[i].plant.sellPrice * orders[i].quantity);
            //nameTxt.text = recipeName;
            //quantityTxt[i].text = "" + orders[i].quantity;
            //icon[i].sprite = orders[i].plant.icon;
        }

        nameTxt.text = recipeName;
        priceTxt.text = "$" + price;

    }
}