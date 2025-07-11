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

    public Dictionary<Vehicule, int> vehiculeList = new();
    
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
                prix = v.prix
            };
            vehiculeList.Add(vehicule, 999);
        }

        isLoaded = true;
        OnLoadComplete?.Invoke();
        InitAdderDisplay();
    }
    
    private void InitAdderDisplay()
    {

        foreach (KeyValuePair<Vehicule, int> kvp in vehiculeList)
        {
            var go = Instantiate(vehiculePrefab, contentPanel);
            go.name = kvp.Key.nom;
            var al = go.GetComponent<AdderLine>();
            al.vehicule = kvp.Key;
            al.usine = null; //ça devrait l'être de base mais je sais pas pourquoi ça l'est pas
            al.text1.text = kvp.Key.nom;
            al.text2.text = kvp.Key.prix + " pièces";
            al.text3.text = "0 acheté.s";
            al.text4.text = "0 disponible.s";

            kvp.Key.adderLine = al;

        }
    }

    public void AcheterVehicule(Vehicule vehicule)
    {
        if (Banque.instance.GetMoney() >= vehicule.prix)
        {
            Banque.instance.AddMoney(-vehicule.prix);

            if (!vehiculeList.ContainsKey(vehicule)) vehiculeList[vehicule] = 0;

            vehiculeList[vehicule]++;
            vehicule.adderLine.text3.text = $"{++vehicule.quantiteAchete} acheté.s";
            vehicule.adderLine.text4.text = $"{++vehicule.quantiteDispo} disponible.s";
        }
    }

    public bool PeutCultiver(Culture culture)
    {
        foreach (var vehicule in culture.vehicules)
            if (!vehiculeList.ContainsKey(vehicule) || vehiculeList[vehicule] <= 0)
                return false;
        return true;    }


    public bool UtiliserVehicules(Culture culture)
    {
        if (!PeutCultiver(culture)) return false;

        foreach (var vehicule in culture.vehicules) vehiculeList[vehicule]--;

        return true;
    }


    public void RendreVehicule(Vehicule vehicule)
    {
        if (vehiculeList.ContainsKey(vehicule))
            vehiculeList[vehicule]++;
    }


    [Serializable]
    private class VehiculeJson
    {
        public string nom;
        public int taille;
        public int prix;
    }
}