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
            if(fm.GetAmount(orders[i].plant) < orders[i].quantity)
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

    public void RemovePlants()
    { 
       for(int i = 0; i < orders.Count; i++)
        {
            //Debug.Log("Sold " + orders[i].quantity + " - "+ orders[i].plant.PlantName);
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

        //TODO fm.AddPlant(new Plant(recipeName,price,orders[0].plant.Icon),1);
    }

    void InitializeUI()
    {
        //Transform HGL = transform.Find("HGL");

        for(int i = 0; i < orders.Count; i++)
        {
            //Add order objects 
            //RecipeObj newR = Instantiate(recipeObj, transform).GetComponent<RecipeObj>();
            //newR.transform.SetParent(HGL);
            HGL.GetChild(i).gameObject.GetComponent<RecipeObj>().InitializeUI(orders[i].quantity,orders[i].plant.Icon);
            //newR.InitializeUI(orders[i].quantity,orders[i].plant.Icon);
            
            //Initialize New Recipe
            price += (orders[i].plant.SellPrice * orders[i].quantity);
            //nameTxt.text = recipeName;
            //quantityTxt[i].text = "" + orders[i].orderQuantity;
            //icon[i].sprite = orders[i].orderPlant.Icon;
        }

        nameTxt.text = recipeName;
        priceTxt.text = "$" + price;

    }
}