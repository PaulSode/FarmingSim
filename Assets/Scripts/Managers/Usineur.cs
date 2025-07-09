using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Usineur : MonoBehaviour
{
    public static Usineur instance;
    public List<UsineJson> usinesList;
    [SerializeField] private GameObject prefabUsine;
    public List<GameObject> _usines = new ();
    
    private void Awake()
    {
        if (instance != null)
        {
            DestroyImmediate(gameObject);
            return;
        }

        instance = this;
        Silo.instance.OnLoadComplete += LoadUsines;
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
            usinesList.Add(v);
        }
    }

    public void CreerUsine(Produit produit)
    {
        var usineACreer = usinesList.Find(element => element.produit == produit);
        if (Banque.instance.GetMoney() < usineACreer.prix) return;
        if (prefabUsine == null)
        {
            Debug.LogError("PrefabChamp n’est pas assigné !");
            return;
        }

        var usine = Instantiate(prefabUsine);
        var usineScript = usine.GetComponent<Usine>();
        usineScript.nom = usineACreer.nom;
        usineScript.entrants = usineACreer.intrants;
        usineScript.produit = usineACreer.produit;
        usineScript.multiplicateur = usineACreer.multiplicateur;
        _usines.Add(usine);
    }
    
    
    [Serializable]
    public class UsineJson
    {
        public string nom;
        public int multiplicateur;
        public List<Culture> intrants;
        public Produit produit;
        public int prix;
    }
}