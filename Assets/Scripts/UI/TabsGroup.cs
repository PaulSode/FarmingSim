using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TabsGroup : MonoBehaviour
{
    private List<TabsButton> tabsButtons = new();
    private TabsButton selectedButton;
    
    public List<GameObject> pages = new();

    public void Subscribe(TabsButton button)
    {
        tabsButtons.Add(button);
    }


    public void OnTabEnter(TabsButton button)
    {
        ResetTabs();
        if (selectedButton == null || selectedButton != button)
            button.background.color = Color.darkGray;
    }

    public void OnTabExit(TabsButton button)
    {
        ResetTabs();
        if (selectedButton == null || selectedButton != button)
            button.background.color = Color.white;
    }

    public void OnTabSelected(TabsButton button)
    { 
        selectedButton = button;
        ResetTabs();  
        button.background.color = Color.skyBlue;
        
        int index = button.transform.GetSiblingIndex();
        for (int i = 0; i < pages.Count; i++)
        {
            if (i == index)
            {
                pages[i].SetActive(true);
            }
            else
            {
                pages[i].SetActive(false);
            }
        }
    }

    private void ResetTabs()
    {
        foreach (var button in tabsButtons.Where(button => selectedButton == null || selectedButton != button))
        {
            button.background.color = Color.white;
        }
    }
}
