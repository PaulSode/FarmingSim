using System;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting.ReorderableList.Element_Adder_Menu;
using UnityEngine;

public class Silo : MonoBehaviour
{
    public static Silo Instance;

    public Dictionary<Culture, int> elements;
    private readonly int maxStockage = 100000;

    [System.Serializable]
    public class CultureJson
    {
        public string nom;
        public int rendement;
        public List<Vehicule> vehicules;
    }

    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
            return;
        }

        Instance = this;
        Garage.Instance.onLoadComplete += LoadCultures;
    }

    public void LoadCultures()
    {
    string path = Path.Combine(Application.streamingAssetsPath, "Cultures.json");

    if (!File.Exists(path))
    {
        Debug.LogError("Fichier cultures.json non trouvé :(");
        return;
    }

    string json = File.ReadAllText(path);

    CultureJson[] cultures = JsonHelper.FromJson<CultureJson>(json);

        foreach (var v in cultures)
        {
            Culture culture = new Culture();
            culture.nom = v.nom;
            culture.rendement = v.rendement;
            culture.vehicules = v.vehicules;
            elements.Add(culture, 0);
        }
    }


    public void AddCulture(Culture culture, int quantite)
    {
        var espaceDisponible = maxStockage - GetTotalQuantite();
        var quantiteAjoutee = Math.Min(quantite, espaceDisponible);

        if (espaceDisponible < 1) Debug.Log("Au secours c'est plein");

        if (!elements.ContainsKey(culture))
        {
            elements.Add(culture, quantiteAjoutee);
        }
        else
        {
            elements[culture] += quantiteAjoutee;
        }
    }


    public int GetTotalQuantite()
    {
        var quantite = 0;
        foreach (var element in elements)
        {
            quantite += element.Value;
        }
        return quantite;
    }

}
