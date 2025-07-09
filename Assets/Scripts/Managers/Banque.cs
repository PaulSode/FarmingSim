using UnityEngine;

public class Banque : MonoBehaviour
{
    public static Banque Instance;

    public int money;

    void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
            return;
        }

        Instance = this;
    }


    public void AddMoney(int quantite)
    {
        if (money + quantite > 0)
        {
            money += quantite;
        }
        else
        {
            Debug.Log("La pauvreté");
        }
    }
}
