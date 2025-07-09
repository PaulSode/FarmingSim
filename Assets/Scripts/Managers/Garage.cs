using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Garage : MonoBehaviour
{
    public static Garage Instance;

    public List<Vehicule> vehiculeList;

    public event Action onLoadComplete;


    [System.Serializable]
    public class VehiculeJson
    {
        public string nom;
        public int taille;
        public int prix;
    }

    void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
            return;
        }

        Instance = this;
        ChargerVehicules();
    }

    private void ChargerVehicules()
    {
    string path = Path.Combine(Application.streamingAssetsPath, "Vehicules.json");

    if (!File.Exists(path))
    {
        Debug.LogError("Fichier vehicules.json non trouvé :(");
        return;
    }

    string json = File.ReadAllText(path);

    VehiculeJson[] vehicules = JsonHelper.FromJson<VehiculeJson>(json);

        foreach (var v in vehicules)
        {
            Vehicule vehicule = new Vehicule();
            vehicule.nom = v.nom;
            vehicule.taille = v.taille;
            vehicule.prix = v.prix;
            vehiculeList.Add(vehicule);
        }
        onLoadComplete?.Invoke();
    }

private Dictionary<Vehicule, int> quantitesVehicules = new();

    public void AcheterVehicule(Vehicule vehicule)
    {
        if (Banque.Instance.money >= vehicule.prix)
        {
            Banque.Instance.AddMoney(-vehicule.prix);

            if (!quantitesVehicules.ContainsKey(vehicule)) quantitesVehicules[vehicule] = 0;

            quantitesVehicules[vehicule]++;
        }
    }

public bool PeutCultiver(Culture culture)
{
    foreach (var vehicule in culture.vehicules)
    {
        if (!quantitesVehicules.ContainsKey(vehicule) || quantitesVehicules[vehicule] <= 0)
            return false;
    }
    return true;
}


    public bool UtiliserVehicules(Culture culture)
{
    if (!PeutCultiver(culture)) return false;

    foreach (var vehicule in culture.vehicules)
    {
        quantitesVehicules[vehicule]--;
    }

    return true;
}


    public void RendreVehicule(Vehicule vehicule)
    {
        if (quantitesVehicules.ContainsKey(vehicule))
            quantitesVehicules[vehicule]++;
    }
}
