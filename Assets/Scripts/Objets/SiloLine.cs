using UnityEngine;

public class SiloLine : MonoBehaviour
{
    public Culture culture;
    public Produit produit;
    
    public void SellItem()
    {
        Silo.instance.AddCulture(culture, -9999999);
        SiloUI.instance.UpdateLine(culture.nom, 0);
    }
    
    public void TrashItem()
    {
        Silo.instance.AddProduit(produit, -9999999);
        SiloUI.instance.UpdateLine(produit.nom, 0);
        ;
    }
}
