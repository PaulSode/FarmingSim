using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TabsGroup : MonoBehaviour
{
    private List<TabsButton> tabsButtons = new();
    
    [SerializeField]
    private TabsButton selectedButton;
    
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
        button.background.color = Color.cyan;

        foreach (var b in tabsButtons.Where(b => b != null && b.linkedPage != null))
        {
            if (b == button)
                b.linkedPage.transform.SetAsLastSibling();
            else
                b.linkedPage.transform.SetSiblingIndex(0);
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
