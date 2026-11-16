using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeObj : MonoBehaviour
{
    public Text quantityTxt;
    public Image icon;

    public void InitializeUI(int qt, Sprite I)
    {    
        quantityTxt.text = "" + qt;
        icon.sprite = I;
    }
       
}
