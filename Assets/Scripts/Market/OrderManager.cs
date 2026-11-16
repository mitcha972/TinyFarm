using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    public GameObject orderItem;
    [SerializeField] private FarmManager fm;
    [SerializeField] private PlantManager pm;

    
    [SerializeField] private Color filledColor;
    [SerializeField] private Color unfilledColor;

    private List<OrderItem> orderItems = new List<OrderItem>();

    [SerializeField] private Transform contentArea;

    private void Awake()
    {
        CreateOrders(); 
        //CheckOrders();       
    }

    public void CreateOrders()
    {
        for(int i = 0; i < 5; i++)
        {
            //Pick Random Number of Items In Order
            //int ranO = Random.Range(1,3);

            OrderItem newOrder = Instantiate(orderItem, contentArea).GetComponent<OrderItem>();

            //Precent Increase for Order
            newOrder.precent = Random.Range(-10,16);
            
            int picked = -1;

            for(int j = 0; j < Random.Range(1,3); j++) //Pick Random Number of Items In Order
            {
                int ran = Random.Range(0,pm.plantObjects.Count);

                if(ran != picked)
                {
                    picked = ran;
                    Plant plant = pm.plantObjects[ran];

                    newOrder.orders.Add(new Order(plant,Random.Range(1,5)));//Pick Random Order Amount
                    orderItems.Add(newOrder);
                }
            }
        }        
    }

    public void CheckOrders()
    {
        //Check is invetory has enough for order
        foreach(OrderItem OI in orderItems)
            OI.CheckOrderItem();
    }


    public void RefreshOrders()
    {
        foreach(OrderItem OI in orderItems)
            Destroy(OI.gameObject);

        orderItems.Clear();
        CreateOrders();
    }
}

