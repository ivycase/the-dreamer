using TMPro;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public TMP_Text coin_label;

    public int totalCoins;
    public int costPerSpin;

    private void Start()
    {
        UpdateLabel();
    }

    private void UpdateLabel()
    {
        coin_label.text = "§" + totalCoins.ToString();
    }

    public void AddCoins(int amount)
    {
        totalCoins += amount;
        UpdateLabel();
    }
}
