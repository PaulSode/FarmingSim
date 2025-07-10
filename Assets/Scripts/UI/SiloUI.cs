using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SiloUI : MonoBehaviour
{
    public static SiloUI instance;
    
    
    [SerializeField]
    private Transform contentPanel;
    
    [SerializeField]
    private GameObject culturePrefab;
    
    [SerializeField]
    private GameObject produitPrefab;
    
    [SerializeField]
    private ScrollRect scrollrect;
    
    private List<GameObject> _lines = new();


    private void Awake()
    {
        if (instance != null)
        {
            DestroyImmediate(gameObject);
            return;
        }
        instance = this;
    }

    private void Start()
    {
        if (Silo.instance.isLoaded)
        {  
            InitSiloDisplay();
        }
        else
        {
            Silo.instance.OnLoadComplete += InitSiloDisplay;
        }
    }

    private void ClearDisplay()
    {
        foreach (Transform child in contentPanel)
        {
            Destroy(child.gameObject);
        }
    }

    private void InitSiloDisplay()
    {
        ClearDisplay();

        foreach (KeyValuePair<Culture, int> kvp in Silo.instance.cultures)
        {
            var go = Instantiate(culturePrefab, contentPanel);
            go.name = kvp.Key.nom;
            go.GetComponent<SiloLine>().culture = kvp.Key;
            var text = go.GetComponentInChildren<TMP_Text>();
            text.text = $"{kvp.Key.nom}\nQuantité : {kvp.Value}";
            
            _lines.Add(go);
        }

        foreach (KeyValuePair<Produit, int> kvp in Silo.instance.produits)
        {
            var go = Instantiate(produitPrefab, contentPanel);
            go.name = kvp.Key.nom;
            go.GetComponent<SiloLine>().produit = kvp.Key;
            var text = go.GetComponentInChildren<TMP_Text>();
            text.text = $"{kvp.Key.nom}\nQuantité : {kvp.Value}";
            
            _lines.Add(go);
        }
        
        scrollrect.verticalNormalizedPosition = 1;
    }

    public void UpdateLine(string element, int value)
    {
        foreach (var line in _lines)
        {
            if (line.name != element) continue;
            var text = line.GetComponentInChildren<TMP_Text>();
            text.text = $"{line.name}\nQuantité : {value}";
        }
    }

    public void ResetAllLines()
    {
        foreach (var line in _lines)
        {
            var text = line.GetComponentInChildren<TMP_Text>();
            text.text = $"{line.name}\nQuantité : 0";
        }
    }
}