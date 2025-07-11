using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class Silo : MonoBehaviour
{
    public static Silo instance;
    public bool isLoaded = false;
    public bool autoSell = false;
    
    [SerializeField]
    private int _maxStockage = 100000;

    public Dictionary<Culture, int> cultures =  new ();
    public Dictionary<Produit, int> produits = new ();

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
        if (Garage.instance.isLoaded)
        {
            LoadCultures();
        }
        else
        {
            Garage.instance.OnLoadComplete += LoadCultures;
        }
    }

    public event Action OnLoadComplete;

    public void LoadCultures()
    {
        var path = Path.Combine(Application.streamingAssetsPath, "Cultures.json");

        if (!File.Exists(path))
        {
            Debug.LogError("Fichier cultures.json non trouvé :(");
            return;
        }

        var json = File.ReadAllText(path);

        var culturesList = JsonHelper.FromJson<CultureJson>(json);

        foreach (var v in culturesList)
        {
            var vehicules = new List<Vehicule>();

            foreach (var nom in v.vehicules)
            {
                var match = Garage.instance.vehiculeList.FirstOrDefault(vh => vh.nom == nom);

                if (match != null)
                    vehicules.Add(match);
                else
                    Debug.LogWarning($"Véhicule '{nom}' introuvable pour la culture '{v.nom}'");
            }


            var culture = new Culture
            {
                nom = v.nom,
                rendement = v.rendement,
                vehicules = vehicules
            };
            cultures.Add(culture, 0);
        }

        LoadProduits();
    }


    public void AddCulture(Culture culture, int quantite)
    {
        var espaceDisponible = _maxStockage - GetTotalQuantite();
        var quantiteAjoutee = 0;
        quantiteAjoutee = quantite >= 0 ? Math.Min(quantite, espaceDisponible) : Math.Max(quantite, -GetCultureValue(culture));

        if (!cultures.TryAdd(culture, quantiteAjoutee))
        {
            cultures[culture] += quantiteAjoutee;
            SiloUI.instance.UpdateLine(culture.nom, cultures[culture]);
        }
    }

    public int GetCultureValue(Culture culture)
    {
        return cultures[culture];
    }

    private void LoadProduits()
    {
        var path = Path.Combine(Application.streamingAssetsPath, "Produits.json");

        if (!File.Exists(path))
        {
            Debug.LogError("Fichier Produits.json non trouvé :(");
            return;
        }

        var json = File.ReadAllText(path);

        var produitsList = JsonHelper.FromJson<ProduitJson>(json);
        
        foreach (var v in produitsList)
        {
            var produit = new Produit
            {
                nom = v.nom,
                prix = v.prix
            };
            produits.Add(produit, 0);
        }

        isLoaded = true;
        OnLoadComplete?.Invoke();
    }

    public void AddProduit(Produit produit, int quantite)
    {
        if (!autoSell)
        {
            var espaceDisponible = _maxStockage - GetTotalQuantite();
            var quantiteAjoutee = 0;
            quantiteAjoutee = quantite >= 0 ? Math.Min(quantite, espaceDisponible) : Math.Max(quantite, -GetProduitValue(produit));

            if (!produits.TryAdd(produit, quantiteAjoutee))
            {
                if (quantiteAjoutee < 0)
                    Banque.instance.AddMoney(produit.prix * -quantiteAjoutee);
                
                produits[produit] += quantiteAjoutee;
                SiloUI.instance.UpdateLine(produit.nom, produits[produit]);
            }
        }
        else
        {
            SellProduct(produit, quantite);
            SiloUI.instance.UpdateLine(produit.nom, produits[produit]);

        }
    }
    
    public int GetProduitValue(Produit produit)
    {
        return produits[produit];
    }

    public void SellProduct(Produit produit, int quantite)
    {
        produits[produit] -= quantite;
        Banque.instance.AddMoney(produit.prix * quantite);
    }

    public void SellAllProducts()
    {
        foreach (var kvp in produits.ToList())
        {
            var produit = kvp.Key;
            var quantite = kvp.Value;

            if (quantite <= 0) continue;
            Banque.instance.AddMoney(produit.prix * quantite);
            produits[produit] = 0;
        }
    }


    public int GetTotalQuantite()
    {
        return cultures.Sum(element => element.Value) 
               + produits.Sum(element => element.Value);
    }

    public bool HasSpace()
    {
        return GetTotalQuantite() < _maxStockage;
    }

    public void ToggleAutoSell()
    {
        autoSell = !autoSell;
        if (autoSell)
        {
            SellAllProducts();
        }
    }


    [Serializable]
    private class CultureJson
    {
        public string nom;
        public int rendement;
        public List<string> vehicules;
    }

    [Serializable]
    private class ProduitJson
    {
        public string nom;
        public int prix;
    }
}