using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Champs : MonoBehaviour
{
    public int number;
    public Etat state;

    [SerializeField] private Button stateButton;

    public Culture culture;

    public Lot lot;

    public float cooldown;

    public enum Etat
    {
        Recolte,
        Laboure,
        Seme,
        Fertilise,
        Pret
    }

    private void Start()
    {
        state = Etat.Recolte;
        cooldown = 0f;
    }

    private void Update()
    {
        if (cooldown > 0f)
        {
            cooldown -= Time.deltaTime;
            if (cooldown <= 0f) state = Etat.Pret;
        } 
    }

    public void Labourer()
    {
        if (state == Etat.Recolte)
        {
            state = Etat.Laboure;
        }
    }

    public void Semer()
    {
        if (state == Etat.Laboure)
        {
            state = Etat.Seme;
            cooldown = 60f;
        }
    }

    public void Fertiliser()
    {
        if (state == Etat.Seme)
        {
            state = Etat.Fertilise;
            cooldown /= 2;
        }
    }
    
    public void Recolter()
    {
        if (cooldown < 0)
        {
            if (state == Etat.Pret || state == Etat.Fertilise)
            {
                state = Etat.Recolte;
                Silo.Instance.AddCulture(culture, culture.rendement);
            } 
        }    
    }
}
