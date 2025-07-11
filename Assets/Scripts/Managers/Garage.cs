using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class Garage : MonoBehaviour
{
    public static Garage instance;
    public bool isLoaded =  false;
    [SerializeField] private Transform contentPanel;
    [SerializeField] private GameObject vehiculePrefab;

    public List<Vehicule> vehiculeList = new();
    
    private void Awake()
    {
        if (instance != null)
        {
            DestroyImmediate(gameObject);
            return;
        }

        instance = this;
        ChargerVehicules();
    }

    public event Action OnLoadComplete;

    private void ChargerVehicules()
    {
        var path = Path.Combine(Application.streamingAssetsPath, "Vehicules.json");

        if (!File.Exists(path))
        {
            Debug.LogError("Fichier vehicules.json non trouvé :(");
            return;
        }

        var json = File.ReadAllText(path);

        var vehicules = JsonHelper.FromJson<VehiculeJson>(json);

        foreach (var v in vehicules)
        {
            var vehicule = new Vehicule
            {
                nom = v.nom,
                taille = v.taille,
                prix = v.prix,
                quantiteAchete = 0,
                quantiteDispo = 0,
            };
            vehiculeList.Add(vehicule);
        }

        isLoaded = true;
        OnLoadComplete?.Invoke();
        InitAdderDisplay();
    }
    
    private void InitAdderDisplay()
    {

        foreach (Vehicule kvp in vehiculeList)
        {
            var go = Instantiate(vehiculePrefab, contentPanel);
            go.name = kvp.nom;
            var al = go.GetComponent<AdderLine>();
            al.vehicule = kvp;
            al.usine = null; //ça devrait l'être de base mais je sais pas pourquoi ça l'est pas 
            al.text1.text = kvp.nom;
            al.text2.text = kvp.prix + " pièces";
            al.text3.text = "0 acheté.s";
            al.text4.text = "0 disponible.s";

            kvp.adderLine = al;

        }
    }

    public void AcheterVehicule(Vehicule vehicule)
    {
        if (Banque.instance.GetMoney() >= vehicule.prix)
        {
            Banque.instance.AddMoney(-vehicule.prix);

            vehicule.adderLine.text3.text = $"{++vehicule.quantiteAchete} acheté.s";
            vehicule.adderLine.text4.text = $"{++vehicule.quantiteDispo} disponible.s";
        }
    }

    public bool PeutCultiver(Culture culture)
    {
        foreach (var vehicule in culture.vehicules)
            if (vehicule.quantiteDispo <= 0)
                return false;
        return true;    
    }


    public void UtiliserVehicules(Culture culture)
    {
        if (PeutCultiver(culture))
        {
            foreach (var vehicule in culture.vehicules) vehicule.adderLine.text4.text = $"{--vehicule.quantiteDispo} disponible.s";

        }
    }


    public void RendreVehicules(Culture culture)
    {
        foreach (var vehicule in culture.vehicules) vehicule.adderLine.text4.text = $"{++vehicule.quantiteDispo} disponible.s";
    }


    [Serializable]
    private class VehiculeJson
    {
        public string nom;
        public int taille;
        public int prix;
    }
}