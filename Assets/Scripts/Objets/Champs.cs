using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Champs : MonoBehaviour
{
    public enum Etat
    {
        Recolte,
        Laboure,
        LaboureFin,
        Seme,
        Fertilise,
        Pret
    }

    public int number;
    public Etat state;

    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text cultureText;
    [SerializeField] private Button progressButton;
    [SerializeField] private Image progressBar;

    private float _startCooldown;
    public float cooldown;

    public Culture culture;

    public Lot lot;

    private void Start()
    {
        state = Etat.Recolte;
        cooldown = 0f;
        _startCooldown = 0f;
        nameText.text = $"Ferme #{number}";
        cultureText.text = culture.nom;

    }

    private void Update()
    {
        if (cooldown <= 0f) return;
        
        cooldown -= Time.deltaTime;
        if (cooldown <= 0f)
        {
            state = state switch
            {
                Etat.Seme or Etat.Fertilise => Etat.Pret,
                Etat.Laboure => Etat.LaboureFin,
                _ => state
            };
            UpdateProgressButton(state);
        }
        progressBar.fillAmount = cooldown / _startCooldown;
    }

    public void Labourer()
    {
        if (state is Etat.Recolte)
        {
            state = Etat.Laboure;
            cooldown = 10f;
            _startCooldown = 10f;
            UpdateProgressButton(state);
        }
    }

    public void Semer()
    {
        if (state is Etat.LaboureFin)
        {
            state = Etat.Seme;
            cooldown = 60f;
            _startCooldown = 60f;
            UpdateProgressButton(state);

        }
        
    }

    public void Fertiliser()
    {
        if (state is Etat.Seme)
        {
            state = Etat.Fertilise;
            cooldown /= 2;
            UpdateProgressButton(state);

        }
    }

    public void Recolter()
    {
        if (cooldown < 0)
            if (state is Etat.Pret)
            {
                state = Etat.Recolte;
                Silo.instance.AddCulture(culture, culture.rendement);
                UpdateProgressButton(state);
            }
    }


    private void UpdateProgressButton(Etat state)
    {
        progressButton.onClick.RemoveAllListeners();

        progressButton.interactable = true;

        var buttonText = progressButton.GetComponentInChildren<TMP_Text>();

        switch (state)
        {
            case Etat.Recolte:
                buttonText.text = "Labourer";
                progressButton.onClick.AddListener(Labourer);
                break;

            case Etat.Laboure:
                buttonText.text = "Semer";
                progressButton.interactable = false;
                break;
            
            case Etat.LaboureFin:
                progressButton.onClick.AddListener(Semer);
                progressButton.interactable = true;
                break;

            case Etat.Seme:
                buttonText.text = "Fertiliser";
                progressButton.onClick.AddListener(Fertiliser);
                break;

            case Etat.Fertilise:
                progressButton.interactable = false;
                break;
            
            case Etat.Pret:
                buttonText.text = "Récolter";
                progressButton.onClick.AddListener(Recolter);
                break;

            default:
                buttonText.text = "Inconnu";
                progressButton.interactable = false;
                break;
        }
    }

}