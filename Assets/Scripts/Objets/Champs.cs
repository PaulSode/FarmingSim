using System;
using System.Collections.Generic;
using System.Linq;
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
    private int _rendementTemporaire;

    public Culture culture;

    public Lot lot;

    private void Start()
    {
        state = Etat.Recolte;
        cooldown = 0f;
        _startCooldown = 0f;
        nameText.text = $"Ferme #{number}";
        cultureText.text = culture.nom;
        progressBar.fillAmount = 0f;
        _rendementTemporaire = culture.rendement;
    }

    private void Update()
    {
        if (cooldown <= 0 || (state == Etat.Seme && cooldown > 0)) progressButton.interactable = LookForAvailability();

        if (cooldown <= 0f || !Silo.instance.HasSpace()) return;
        
        cooldown -= Time.deltaTime;
        if (cooldown <= 0f)
        {
            cooldown = 0f;
            switch (state)
            {
                case Etat.Laboure:
                    LibererVehiculesEtape("Labourer");
                    state = Etat.LaboureFin;
                    break;

                case Etat.Seme:
                    LibererVehiculesEtape("Semer");
                    state = Etat.Pret;
                    break;

                case Etat.Fertilise:
                    LibererVehiculesEtape("Fertiliser");
                    state = Etat.Pret;
                    break;
                case Etat.Pret:
                    LibererVehiculesEtape("Recolter");
                    Silo.instance.AddCulture(culture, _rendementTemporaire);
                    _rendementTemporaire = culture.rendement;
                    state = Etat.Recolte;
                    break;
            }
            UpdateProgressButton(state);
        }
        progressBar.fillAmount = cooldown / _startCooldown;
    }

    public void Labourer()
    {
        if (state == Etat.Recolte)
        {
            if (!ReserverVehiculesEtape("Labourer"))
            {
                Debug.Log("Pas de véhicule pour labourer !");
                return;
            }

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
            if (!ReserverVehiculesEtape("Semer"))
            {
                Debug.Log("Pas de véhicule pour labourer !");
                return;
            }
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
            if (!ReserverVehiculesEtape("Fertiliser"))
            {
                Debug.Log("Pas de véhicule pour labourer !");
                return;
            }
            LibererVehiculesEtape("Semer");
            state = Etat.Fertilise;
            cooldown += 20f;
            _startCooldown += 20f;
            _rendementTemporaire = culture.rendement * 2;
            UpdateProgressButton(state);

        }
    }

    public void Recolter()
    {
        if (cooldown <= 0)
            if (state is Etat.Pret)
            {
                if (!ReserverVehiculesEtape("Recolter"))
                {
                    Debug.Log("Pas de véhicule pour labourer !");
                    return;
                }
                cooldown = 10f;
                _startCooldown = 10f;
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
                progressButton.interactable = false;
                break;

            default:
                buttonText.text = "Inconnu";
                progressButton.interactable = false;
                break;
        }
    }
    
    public bool ReserverVehiculesEtape(string etape)
    {
        Debug.Log(culture.vehicules[3].nom);
        Debug.Log(culture.GetVehiculesPourEtape(etape)[0].nom + ' ' +
                  culture.GetVehiculesPourEtape(etape)[0].quantiteDispo);
        
        var liste = culture.GetVehiculesPourEtape(etape);

        foreach (var v in liste)
            if (v.quantiteDispo <= 0) return false;

        foreach (var v in liste)
        {
            v.quantiteDispo--;
            v.adderLine.text4.text = $"{v.quantiteDispo} disponible.s";
        }

        return true;
    }

    public void LibererVehiculesEtape(string etape)
    {
        var liste = culture.GetVehiculesPourEtape(etape);

        foreach (var v in liste)
        {
            v.quantiteDispo++;
            v.adderLine.text4.text = $"{v.quantiteDispo} disponible.s";
        }
    }

    private bool LookForAvailability()
    {
        var liste = new List<Vehicule>();
        switch (state)
        {
            case Etat.Recolte:
                liste = culture.GetVehiculesPourEtape("Labourer");
                break;
            case Etat.LaboureFin:
                liste = culture.GetVehiculesPourEtape("Semer");
                break;
            case Etat.Seme:
                liste = culture.GetVehiculesPourEtape("Fertiliser");
                break;
            case Etat.Pret:
                liste = culture.GetVehiculesPourEtape("Recolter");
                break;

            default:
                break;
        }
        return liste.All(v => v.quantiteDispo >= 1);
    }


}