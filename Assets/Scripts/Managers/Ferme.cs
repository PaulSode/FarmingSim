using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Ferme : MonoBehaviour
{
    public static Ferme Instance;

    [SerializeField] private GameObject prefabChamp;
    private List<GameObject> champs = new List<GameObject>();

    void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
            return;
        }

        Instance = this;
    }


    public void CreerChamps(Culture culture)
    {
        if (!Garage.Instance.UtiliserVehicules(culture))
        {
            Debug.Log("Pas assez de véhicules pour cette culture.");
            return;
        }

        if (prefabChamp == null)
        {
            Debug.LogError("PrefabChamp n’est pas assigné !");
            return;
        }

        GameObject champ = Instantiate(prefabChamp);
        champs.Add(champ);

        champ.GetComponent<Champs>().culture = culture;
    }
}
