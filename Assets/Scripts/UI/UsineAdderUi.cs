using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UsineAdderUI : MonoBehaviour
{
    [SerializeField] private Transform contentPanel;
    [SerializeField] private GameObject usinePrefab;
    
    private void Start()
    {
        if (Usineur.instance.isLoaded)
        {
            InitAdderDisplay();
        }
        else
        {
            Usineur.instance.OnLoadComplete += InitAdderDisplay;
        }
    }

    private void InitAdderDisplay()
    {

        foreach (var kvp in Usineur.instance.usinesJson)
        {
            var go = Instantiate(usinePrefab, contentPanel);
            go.name = kvp.nom;
            var al = go.GetComponent<AdderLine>();
            al.usine = kvp;
            al.text1.text = kvp.nom;
            al.text2.text = kvp.produit;
            if (kvp.intrants is { Count: > 0 })
            {
                al.text3.text = kvp.intrants.Aggregate("", (current, v) => current + $"{v}\n");
            }
            al.text4.text = $"Multiplicateur : {kvp.multiplicateur}x";
            al.text5.text = kvp.prix + " pièces";
        }
        ToggleVisibility();
    }


    public void ToggleVisibility()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
