using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AdderLine : MonoBehaviour
{
    public TMP_Text text1;
    public TMP_Text text2;
    public TMP_Text text3;
    public TMP_Text text4;
    public TMP_Text text5;
    
    public Culture culture;
    public Usineur.UsineJson usine;
    public Vehicule vehicule;
    
    [SerializeField] private Button addButton;

    private float _cooldown = 1f;

    private void Update()
    {
        _cooldown -= Time.deltaTime;
        if (_cooldown <= 0f)
        {
            UpdateButton();
            _cooldown = 1f;
        }
    }

    public void Choose()
    {
        if (culture != null)
        {
            Ferme.instance.CreerChamps(culture);
        }
        else if (usine != null)
        {
            Usineur.instance.CreerUsine(usine.produit);
        }
        else
        {
            Garage.instance.AcheterVehicule(vehicule);
        }
    }

    private void UpdateButton()
    {
        if (usine != null)
        {
            addButton.interactable = Banque.instance.GetMoney() >= usine.prix;
        } else if (vehicule != null)
        {
            addButton.interactable = Banque.instance.GetMoney() >= vehicule.prix;
        }
    }
}
