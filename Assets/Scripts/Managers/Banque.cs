using UnityEngine;

public class Banque : MonoBehaviour
{
    public static Banque instance;

    [SerializeField]
    private int money = 10000;

    private void Awake()
    {
        if (instance != null)
        {
            DestroyImmediate(gameObject);
            return;
        }

        instance = this;
    }


    public void AddMoney(int quantite)
    {
        if (money + quantite >= 0)
            money += quantite;
        else
            Debug.Log("La pauvreté");
    }

    public int GetMoney()
    {
        return money;
    }
}