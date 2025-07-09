using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NouvelleCulture", menuName = "Culture")]
public class Culture : ScriptableObject
{
    public string nom;
    public int rendement;

    public List<Vehicule> vehicules;

}
