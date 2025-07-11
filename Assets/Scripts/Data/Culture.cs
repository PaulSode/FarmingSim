using System.Collections.Generic;

public class Culture
{
    public string nom;
    public int rendement;

    public List<Vehicule> vehicules = new ();
    
    public List<Vehicule> GetVehiculesPourEtape(string etape)
    {
        return etape switch
        {
            "Labourer" => new List<Vehicule> { vehicules[0] },
            "Semer" => new List<Vehicule> { vehicules[1] },
            "Fertiliser" => new List<Vehicule> { vehicules[0] },
            "Recolter" => new List<Vehicule> { vehicules[2], vehicules[3] },
            _ => new List<Vehicule>()
        };
    }

}