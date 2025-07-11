using System.Linq;
using UnityEngine;

public class Animal
{
   public int eau = 10;
   public int taille = 1;

   public void Die()
   {
      var viande = Silo.instance.produits.Keys.First(p => p.nom == "Viande");
      Silo.instance.AddProduit(viande, taille);
   }
}
