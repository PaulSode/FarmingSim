using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Garage : MonoBehaviour
{
    public static Garage instance;
    public bool isLoaded =  false;

    private Dictionary<Vehicule, int> _quantitesVehicules = new();

    public List<Vehicule> vehiculeList = new ();

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
            vehiculeList.Add(vehicule);
        }

        isLoaded = true;
        OnLoadComplete?.Invoke();
    }

    public void AcheterVehicule(Vehicule vehicule)
    {
        if (Banque.instance.GetMoney() >= vehicule.prix)
        {
            Banque.instance.AddMoney(-vehicule.prix);

            if (!_quantitesVehicules.ContainsKey(vehicule)) _quantitesVehicules[vehicule] = 0;

            _quantitesVehicules[vehicule]++;
        }
    }

    public bool PeutCultiver(Culture culture)
    {
        foreach (var vehicule in culture.vehicules)
            if (!_quantitesVehicules.ContainsKey(vehicule) || _quantitesVehicules[vehicule] <= 0)
                return false;
        return true;
    }


    public bool UtiliserVehicules(Culture culture)
    {
        if (!PeutCultiver(culture)) return false;

        foreach (var vehicule in culture.vehicules) _quantitesVehicules[vehicule]--;

        return true;
    }


    public void RendreVehicule(Vehicule vehicule)
    {
        if (_quantitesVehicules.ContainsKey(vehicule))
            _quantitesVehicules[vehicule]++;
    }


    [Serializable]
    private class VehiculeJson
    {
        public string nom;
        public int taille;
        public int prix;
    }
}