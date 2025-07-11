using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Mathematics.Geometry;
using UnityEngine;
using UnityEngine.UI;

public class Usine : MonoBehaviour
{
    public enum Travail
    {
        Production,
        Attente,
        Pause
    }

    public int number;
    public string nom;
    public int multiplicateur;

    public Travail status;

    private float _cooldown;
	private float _startCooldown;
    private int minEntrant;
    public bool hasManager = false;
    
    public List<Usineur.Intrant> entrants;
    public Produit produit;

    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text usineText;
    [SerializeField] private Button progressButton;
    [SerializeField] private Image progressBar;

    private void Start()
    {
        status = Travail.Attente;
        _cooldown = 0f;
        _startCooldown = 0f;
        nameText.text = $"Usine #{number}";
        usineText.text = nom;
        progressBar.fillAmount = 0f;
    }

    private void Update()
    {
        if (!Silo.instance.HasSpace()) status = Travail.Pause;
        
        if (_cooldown > 0 && status == Travail.Production)
        {
            _cooldown -= Time.deltaTime;
            progressBar.fillAmount = _cooldown / _startCooldown;

        }
        else if (status == Travail.Pause)
        {
            //En vrai faudrait faire un event quand le silo est update mais j'ai pas le temps lol
            if (PeutProduire()) status = Travail.Production;
        } else if (status == Travail.Attente)
        {
            //En vrai faudrait faire un event quand le silo est update mais j'ai pas le temps lol
            PeutProduire();
        }
        else
        {
            FinirProduire();
            if (hasManager) CommencerProduire();
        }
    }

    public void CommencerProduire()
    {
        if (!PeutProduire()) return;

        progressButton.interactable = false;
        status = Travail.Production;
        _cooldown = 1f;

        var quantites = entrants.Select(i => i.type switch
        {
            Usineur.TypeIntrant.Culture => Silo.instance.GetCultureValue(i.culture),
            Usineur.TypeIntrant.Produit => Silo.instance.GetProduitValue(i.produit),
            _ => 0
        }).ToList();

        if (quantites.Any(q => q <= 0)) return;

        minEntrant = Mathf.Min(quantites.Min(), 100);

        foreach (var i in entrants)
        {
            if (i.type == Usineur.TypeIntrant.Culture)
                Silo.instance.AddCulture(i.culture, -minEntrant);
            else if (i.type == Usineur.TypeIntrant.Produit)
                Silo.instance.AddProduit(i.produit, -minEntrant);
        }
    }

    private void FinirProduire()
    {
        progressButton.interactable = true;
        status = Travail.Attente;
        Silo.instance.AddProduit(produit, minEntrant * multiplicateur);
    }
    
    private bool PeutProduire()
    {
        var quantites = entrants.Select(i => i.type switch
        {
            Usineur.TypeIntrant.Culture => Silo.instance.GetCultureValue(i.culture),
            Usineur.TypeIntrant.Produit => Silo.instance.GetProduitValue(i.produit),
            _ => 0
        });

        if (quantites.All(q => q > 0) && Silo.instance.HasSpace())
        {
            progressButton.interactable = true;
            return true;
        }
        else
        {
            progressButton.interactable = false;
            return false;
        }
    }

}