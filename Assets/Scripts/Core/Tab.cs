using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tab : MonoBehaviour
{   
    public string tabTitle;
    public int tabNum;
    public Image buttonsImg;
    public int currentPage = 0;
    public List<GameObject> pages;    
}

