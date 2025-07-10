using UnityEngine;

public class SiloLine : MonoBehaviour
{
    public Culture culture;
    public Produit produit;
    
    public void SellItem()
    {
        Silo.instance.AddProduit(produit, -9999999);
    }
    
    public void TrashItem()
    {
        Silo.instance.AddCulture(culture, -9999999);
    }
}
