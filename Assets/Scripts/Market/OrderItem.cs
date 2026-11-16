using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderItem : MonoBehaviour
{
    private FarmManager fm;

    [SerializeField] private Text precentTxt;
    [SerializeField] private Text priceTxt;
    //[SerializeField] private Text nameTxt;
    [SerializeField] private Text btnTxt;
    [SerializeField] private GameObject recipeObj;
    [SerializeField] private Transform orderSlot;

    public Button btn;

    public Image bkg;

    private int price; 
    public int precent;

    public List<Order> orders;   

    void Start()
    {
        fm = FindObjectOfType<FarmManager>();
        InitializeUI();
        CheckOrderItem();
    }

    public void CheckOrderItem()
    {  
        for(int i = 0; i < orders.Count; i++){      
            if(PlantQuantity(orders[i].plant) >= orders[i].quantity)
            {
                orderSlot.GetChild(i).gameObject.GetComponent<RecipeObj>().quantityTxt.color  = Color.green;  
            }
            else
            {
                orderSlot.GetChild(i).gameObject.GetComponent<RecipeObj>().quantityTxt.color  = Color.red; 
            }
        }
    }

    public void CheckOrder()
    {  
        for(int i = 0; i < orders.Count; i++){      
            if(PlantQuantity(orders[i].plant) < orders[i].quantity){
                Debug.Log("noFire");
                return;                
            }
        }

        btnTxt.text = "Complete";
        var tempColor = bkg.color;
        tempColor.a = 0.3f;  
        bkg.color = tempColor;  
        btn.interactable = false;  
        Sell();        
    }

    private int PlantQuantity(Plant plant)
    {
        for(int i = 0; i < fm.plantInventory.Count; i++){
            if(fm.plantInventory[i].plantName == plant.plantName)
                return fm.plantInventory[i].quantity;
        }

        return 0;
    }   

    public void Sell()
    { 
        for(int i = 0; i < orders.Count; i++)
        {
            Debug.Log("Sold " + orders[i].plant.plantName);
            fm.RemovePlant(orders[i].plant, orders[i].quantity);
        }

        fm.Transaction(price);
    }

    void InitializeUI()
    {
        for(int i = 0; i < orders.Count; i++)
        {
            //RecipeObj newR = Instantiate(recipeObj, transform).GetComponent<RecipeObj>();
            //newR.transform.SetParent(orderSlot);
            orderSlot.GetChild(i).gameObject.GetComponent<RecipeObj>().InitializeUI(orders[i].quantity,orders[i].plant.icon);

            price += orders[i].plant.sellPrice*orders[i].quantity;
        }

        float p = price;
        //Debug.Log(p +"  "+precent/100);
        price += Mathf.RoundToInt(p * (precent/100f)); 
        priceTxt.text = "$" + price;
        precentTxt.text = precent + "%";
    }
}