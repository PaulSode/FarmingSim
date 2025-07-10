using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class Garage : MonoBehaviour
{
    public static Garage instance;
    public bool isLoaded =  false;

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
    }

    public void AcheterVehicule(Vehicule vehicule)
    {
        if (Banque.instance.GetMoney() >= vehicule.prix)
        {
            Banque.instance.AddMoney(-vehicule.prix);

            if (!vehiculeList.ContainsKey(vehicule)) vehiculeList[vehicule] = 0;

            vehiculeList[vehicule]++;
        }
    }

    public bool PeutCultiver(Culture culture)
    {
        return culture.vehicules.All(vehicule => vehiculeList.ContainsKey(vehicule) && vehiculeList[vehicule] > 0);
    }


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