using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngredientSlot : MonoBehaviour
{
    public Text quantityTxt;
    public Image icon;

    public void InitializeUI(int qt, Sprite I)
    {    
        quantityTxt.text = "" + qt;
        icon.sprite = I;
    }
       
}
