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
            "Labourer" => new List<Vehicule> { vehicules[0] },                     // Tracteur
            "Semer" => new List<Vehicule> { vehicules[1] },                        // Semeuse / Planteuse
            "Fertiliser" => new List<Vehicule> { vehicules[4] },                   // Fertilisateur
            "Recolter" => new List<Vehicule> { vehicules[2], vehicules[3] },      // Moissonneuse + Remorque
            _ => new List<Vehicule>()
        };
    }


}