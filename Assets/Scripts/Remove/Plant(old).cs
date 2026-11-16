using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Plantt
{
    public string plantName;
    public int sellPrice;
    public int quantity;
    public Sprite icon;

    public Plantt(string pName, int sPrice, Sprite con)
    {
        plantName = pName;
        sellPrice = sPrice;
        icon = con;
        quantity = 1;
    }    
}