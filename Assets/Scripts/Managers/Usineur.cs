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

    public void CreerUsine(Produit produit)
    {
        var usineACreer = usinesJson.Find(element => element.produit == produit.nom);
        if (Banque.instance.GetMoney() < usineACreer.prix) return;
        if (prefabUsine == null)
        {
            Debug.LogError("PrefabChamp n’est pas assigné !");
            return;
        }

        var usine = Instantiate(prefabUsine, contentPanel);
        var usineScript = usine.GetComponent<Usine>();
        usineScript.nom = usineACreer.nom;
        
        foreach (var nom in usineACreer.intrants)
        {
            var match = Silo.instance.produits.Keys.FirstOrDefault(vh => vh.nom == nom);

            if (match != null)
                usineScript.produit = match;
            else
                Debug.LogWarning($"Produit '{nom}' introuvable pour l'usine '{usineACreer.nom}'");
        }
        
        var cultures = new List<Culture>();
        foreach (var nom in usineACreer.intrants)
        {
            var match = Silo.instance.cultures.Keys.FirstOrDefault(vh => vh.nom == nom);

            if (match != null)
                cultures.Add(match);
            else
                Debug.LogWarning($"Culture '{nom}' introuvable pour l'usine '{usineACreer.nom}'");
        }
        usineScript.entrants = cultures;
        
        usineScript.multiplicateur = usineACreer.multiplicateur;
        usinesList.Add(usineScript);
    }
    
    
    [Serializable]
    public class UsineJson
    {
        public string nom;
        public int multiplicateur;
        public List<string> intrants;
        public string produit;
        public int prix;
    }
}