using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CultureAdderUI : MonoBehaviour
{
    [SerializeField] private Transform contentPanel;
    [SerializeField] private GameObject culturePrefab;
    
    private void Start()
    {
        if (Silo.instance.isLoaded)
        {
            InitAdderDisplay();
        }
        else
        {
            Silo.instance.OnLoadComplete += InitAdderDisplay;
        }
    }

    private void InitAdderDisplay()
    {

        foreach (KeyValuePair<Culture, int> kvp in Silo.instance.cultures)
        {
            var go = Instantiate(culturePrefab, contentPanel);
            go.name = kvp.Key.nom;
            var al = go.GetComponent<AdderLine>();
            al.culture = kvp.Key;
            al.text1.text = kvp.Key.nom;
            al.text2.text = $"{kvp.Key.rendement}/hectare";

            if (kvp.Key.vehicules is { Count: > 0 })
            {
                var vehiculeText = kvp.Key.vehicules.Aggregate("", (current, v) => current + $"{v.nom}\n");
                al.text3.text = vehiculeText;
            }
        }
        ToggleVisibility();
    }


    public void ToggleVisibility()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
