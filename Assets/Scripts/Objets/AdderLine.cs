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

    public void ChooseCulture()
    {
        Ferme.instance.CreerChamps(culture);
    }
    
    public void ChooseUsine()
    {
        //Usineur.instance.CreerUsine(usine);
    }

    private void UpdateButton()
    {
        if (culture != null)
        {
            addButton.interactable = Garage.instance.PeutCultiver(culture);
        } else if (usine != null)
        {
            addButton.interactable = Banque.instance.GetMoney() >= usine.prix;
        }
    }
}
