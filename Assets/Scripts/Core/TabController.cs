using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabController : MonoBehaviour
{
    public ScrollRect myScrollRect;
    public Text storeTitle;
    public int currentTab = 0;
    public int currentPage = 0;
    public List<Tab> tabs;

    [SerializeField] private Color selectedColor;
    [SerializeField] private Color unselectedColor;

    void Start()
    {
        myScrollRect.content = tabs[currentTab].pages[currentPage].GetComponent<RectTransform>();
        //storeTitle.text = tabs[currentTab].pages[currentPage].name;
    }

    public void SelectTab(int tabNumber)
    {
        if(tabNumber == currentTab)
            return;   

        tabs[currentTab].buttonsImg.color =  unselectedColor;
        tabs[currentTab].pages[currentPage].SetActive(false); 
        tabs[tabNumber].pages[tabs[tabNumber].currentPage].SetActive(true);

        currentTab = tabs[tabNumber].tabNum;
        currentPage = tabs[currentTab].currentPage;
        myScrollRect.content = tabs[currentTab].pages[currentPage].GetComponent<RectTransform>();
        storeTitle.text = tabs[currentTab].pages[currentPage].name;    
        tabs[currentTab].buttonsImg.color =  selectedColor;
    }

    public void NextPage(int num)
    {
        tabs[currentTab].pages[currentPage].SetActive(false);

        int tabPageRange = tabs[currentTab].pages.Count;       

        if (currentPage + num >  tabPageRange-1)  
            tabs[currentTab].currentPage = 0;
        else if (currentPage + num <  tabPageRange-1)
            tabs[currentTab].currentPage = tabPageRange-1;
        else
            tabs[currentTab].currentPage = currentPage + num;

        currentPage = tabs[currentTab].currentPage;
            
        tabs[currentTab].pages[currentPage].SetActive(true);    

        myScrollRect.content = tabs[currentTab].pages[currentPage].GetComponent<RectTransform>();
        storeTitle.text = tabs[currentTab].pages[currentPage].name;       
    }
}
