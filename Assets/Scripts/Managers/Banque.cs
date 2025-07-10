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

    private void Start()
    {
        MoneyUI.instance.UpdateMoney(money);
    }


    public void AddMoney(int quantite)
    {
        if (money + quantite >= 0)
        {
            money += quantite;
            MoneyUI.instance.UpdateMoney(money);
        }
            
        else
            Debug.Log("La pauvreté");
    }

    public int GetMoney()
    {
        return money;
    }
}