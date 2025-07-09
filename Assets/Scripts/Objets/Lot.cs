using System.Collections.Generic;

public class Lot
{
    public List<Champs> champsList;

    public void AddChamps(Champs champs)
    {
        champsList.Add(champs);
    }

    public void RemoveChamps(Champs champs)
    {
        if (champsList.Contains(champs)) champsList.Remove(champs);
    }

    public List<int> GetChampsNumbers()
    {
        var liste = new List<int>();
        foreach (var champs in champsList) liste.Add(champs.number);

        return liste;
    }
}