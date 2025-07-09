using UnityEngine;
using UnityEngine.UI;

public class Champs : MonoBehaviour
{
    public enum Etat
    {
        Recolte,
        Laboure,
        Seme,
        Fertilise,
        Pret
    }

    public int number;
    public Etat state;

    [SerializeField] private Button stateButton;

    public float cooldown;

    public Culture culture;

    public Lot lot;

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
            if (cooldown <= 0f)
                state = state switch
                {
                    Etat.Seme or Etat.Fertilise => Etat.Pret,
                    Etat.Recolte => Etat.Laboure,
                    _ => state
                };
        }
    }

    public void Labourer()
    {
        if (state == Etat.Recolte)
        {
            cooldown = 10f;
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
            if (state is Etat.Pret or Etat.Fertilise)
            {
                state = Etat.Recolte;
                Silo.instance.AddCulture(culture, culture.rendement);
            }
    }
}