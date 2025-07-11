using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class Usineur : MonoBehaviour
{
    public static Usineur instance;
    public bool isLoaded = false;
    [FormerlySerializedAs("_usinesJson")] public List<UsineJson> usinesJson;
    [SerializeField] private GameObject prefabUsine;
    [SerializeField] private Transform contentPanel;

    public List<Usine> usinesList = new ();
    private int _usineNumber = 1;

    
    public event Action OnLoadComplete;
    
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
            LoadUsines();
        }
        else
        {
            Silo.instance.OnLoadComplete += LoadUsines;
        }
    }


    private void LoadUsines()
    {
        var path = Path.Combine(Application.streamingAssetsPath, "Usines.json");

        if (!File.Exists(path))
        {
            Debug.LogError("Fichier Usines.json non trouvé :(");
            return;
        }

        var json = File.ReadAllText(path);

        var usines = JsonHelper.FromJson<UsineJson>(json);

        foreach (var v in usines)
        {
            usinesJson.Add(v);
        }
        
        isLoaded = true;
        OnLoadComplete?.Invoke();
    }

public void CreerUsine(string produit)
{
    var usineACreer = usinesJson.Find(element => element.produit == produit);
    if (usineACreer == null)
    {
        Debug.LogWarning($"Aucune usine trouvée pour le produit '{produit}'");
        return;
    }

    if (Banque.instance.GetMoney() < usineACreer.prix) return;

    if (prefabUsine == null)
    {
        Debug.LogError("PrefabUsine n’est pas assigné !");
        return;
    }

    Banque.instance.AddMoney(-usineACreer.prix);

    var usineGO = Instantiate(prefabUsine, contentPanel);
    var usineScript = usineGO.GetComponent<Usine>();
    usineScript.nom = usineACreer.nom;

    var produitMatch = Silo.instance.produits.Keys
        .FirstOrDefault(p => string.Equals(p.nom, usineACreer.produit, StringComparison.OrdinalIgnoreCase));
    
    if (produitMatch != null)
    {
        usineScript.produit = produitMatch;
    }
    else
    {
        Debug.LogWarning($"Produit '{usineACreer.produit}' introuvable pour l'usine '{usineACreer.nom}'");
    }

    var intrants = new List<Intrant>();

    foreach (var intrantJson in usineACreer.intrants)
    {
        var intrant = new Intrant();

        if (intrantJson.type.Equals("Culture", StringComparison.OrdinalIgnoreCase))
        {
            var match = Silo.instance.cultures.Keys
                .FirstOrDefault(c => string.Equals(c.nom, intrantJson.nom, StringComparison.OrdinalIgnoreCase));
            if (match != null)
            {
                intrant.type = TypeIntrant.Culture;
                intrant.culture = match;
            }
            else
            {
                Debug.LogWarning($"Culture '{intrantJson.nom}' introuvable pour l'usine '{usineACreer.nom}'");
                continue;
            }
        }
        else if (intrantJson.type.Equals("Produit", StringComparison.OrdinalIgnoreCase))
        {
            var match = Silo.instance.produits.Keys
                .FirstOrDefault(p => string.Equals(p.nom, intrantJson.nom, StringComparison.OrdinalIgnoreCase));
            if (match != null)
            {
                intrant.type = TypeIntrant.Produit;
                intrant.produit = match;
            }
            else
            {
                Debug.LogWarning($"Produit '{intrantJson.nom}' introuvable pour l'usine '{usineACreer.nom}'");
                continue;
            }
        }
        else
        {
            Debug.LogWarning($"Type d’intrant invalide : {intrantJson.type}");
            continue;
        }

        intrants.Add(intrant);
    }

    usineScript.entrants = intrants;
    usineScript.multiplicateur = usineACreer.multiplicateur;
    usineScript.number = _usineNumber++;
    usinesList.Add(usineScript);
}

    
    
    [Serializable]
    public class UsineJson
    {
        public string nom;
        public int multiplicateur;
        public List<IntrantJson> intrants;
        public string produit;
        public int prix;
    }
    [Serializable]
    public class IntrantJson
    {
        public string nom;
        public string type; // "Culture" ou "Produit"
    }
    
    public enum TypeIntrant
    {
        Culture,
        Produit
    }

    [Serializable]
    public class Intrant
    {
        public TypeIntrant type;
        public Culture culture;
        public Produit produit;

        public string Nom => type switch
        {
            TypeIntrant.Culture => culture?.nom,
            TypeIntrant.Produit => produit?.nom,
            _ => "Inconnu"
        };
    }
}