using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Order
{
    public Plant plant;
    public int quantity;

    public Order(Plant plant, int quantity)
    {
        this.plant = plant;
        this.quantity = quantity;
    }
}
