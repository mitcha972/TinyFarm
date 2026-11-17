using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabController : MonoBehaviour
{
    [SerializeField] private ScrollRect myScrollRect;
    [SerializeField] private Text storeTitle;
    [SerializeField] private int currentTab;
    [SerializeField] private int currentPage;
    [SerializeField] private List<Tab> tabs;
    [SerializeField] private Color selectedColor;
    [SerializeField] private Color unselectedColor;

    private void Start()
    {
        ShowPage();
    }

    public void SelectTab(int tabNumber)
    {
        if (tabNumber < 0 || tabNumber >= tabs.Count) return;
        if (currentTab >= 0 && currentTab < tabs.Count) tabs[currentTab].currentPage = currentPage;
        currentTab = tabNumber;
        currentPage = tabs[currentTab].currentPage;
        ShowPage();
    }

    public void NextPage(int offset)
    {
        if (currentTab < 0 || currentTab >= tabs.Count) return;
        int count = tabs[currentTab].pages.Count;
        if (count == 0) return;
        currentPage = (int)(((long)currentPage + offset) % count + count) % count;
        ShowPage();
    }

    private void ShowPage()
    {
        if (currentTab < 0 || currentTab >= tabs.Count || tabs[currentTab].pages.Count == 0) return;
        int count = tabs[currentTab].pages.Count;
        currentPage = (currentPage % count + count) % count;
        tabs[currentTab].currentPage = currentPage;
        for (int i = 0; i < tabs.Count; i++)
        {
            tabs[i].buttonsImg.color = i == currentTab ? selectedColor : unselectedColor;
            for (int j = 0; j < tabs[i].pages.Count; j++)
                tabs[i].pages[j].SetActive(i == currentTab && j == currentPage);
        }
        GameObject page = tabs[currentTab].pages[currentPage];
        myScrollRect.content = page.GetComponent<RectTransform>();
        storeTitle.text = page.name;
    }
}
