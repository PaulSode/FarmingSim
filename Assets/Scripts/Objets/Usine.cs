using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Usine : MonoBehaviour
{
    public enum Travail
    {
        Production,
        Attente,
        Pause
    }

    public string nom;
    public int prix;
    public int multiplicateur;

    public Travail status;

    private float _cooldown;
    private int minEntrant;
    public bool hasManager = false;
    
    public List<Culture> entrants;
    public Produit produit;

    private void Start()
    {
        _cooldown = 0f;
        status = Travail.Attente;
    }

    private void Update()
    {
        if (_cooldown > 0 && status == Travail.Production)
        {
            _cooldown -= Time.deltaTime;
        }
        else if (status == Travail.Pause)
        {
            if (PeutProduire()) status = Travail.Production;
        }
        else
        {
            FinirProduire();
            if (hasManager) CommencerProduire();
        }
    }

    private void CommencerProduire()
    {
        if (!PeutProduire()) return;
        
        status = Travail.Production;
        _cooldown = 1f;
        minEntrant = entrants.Select(c => Silo.instance.GetCultureValue(c)).Prepend(100).Min();
        foreach (var c in entrants) Silo.instance.AddCulture(c, -minEntrant);
    }

    private void FinirProduire()
    {
        status = Travail.Attente;
        Silo.instance.AddProduit(produit, minEntrant * multiplicateur);
    }
    
    private bool PeutProduire()
    {
        return entrants.Select(c => Silo.instance.GetCultureValue(c)).Prepend(100).Min() > 0 
               && Silo.instance.HasSpace();
    }
}