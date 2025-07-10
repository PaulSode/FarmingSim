using System.Collections.Generic;
using UnityEngine;

public class Ferme : MonoBehaviour
{
    public static Ferme instance;


    [SerializeField] private GameObject prefabChamp;
    [SerializeField] private Transform contentPanel;
    private List<GameObject> _champs = new();
    private int _champsNumber = 1;


    private void Awake()
    {
        if (instance != null)
        {
            DestroyImmediate(gameObject);
            return;
        }

        instance = this;
    }


    public void CreerChamps(Culture culture)
    {
        if (!Garage.instance.UtiliserVehicules(culture))
        {
            Debug.Log("Pas assez de véhicules pour cette culture.");
            return;
        }

        if (prefabChamp == null)
        {
            Debug.LogError("PrefabChamp n’est pas assigné !");
            return;
        }

        var champ = Instantiate(prefabChamp, contentPanel);
        _champs.Add(champ);

        var go = champ.GetComponent<Champs>();
        go.culture = culture;
        go.number = _champsNumber;      
        _champsNumber++;

    }
}