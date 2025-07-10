using TMPro;
using UnityEngine;

public class MoneyUI : MonoBehaviour
{
    public static MoneyUI instance;
    
    [SerializeField] private TMP_Text moneyText;

    private void Awake()
    {
        if (instance != null)
        {
            DestroyImmediate(gameObject);
            return;
        }
        instance = this;
    }


    public void UpdateMoney(int value)
    {
        moneyText.text = value.ToString();
    }
}
