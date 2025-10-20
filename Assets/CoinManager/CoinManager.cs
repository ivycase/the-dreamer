using TMPro;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public TMP_Text coin_label;
    public TMP_Text event_label;

    public int totalCoins;
    public int costPerSpin;

    private void Start()
    {
        UpdateCoinLabel();
    }

    private void UpdateCoinLabel()
    {
        coin_label.text = "§" + totalCoins;
    }

    // probably make a separate event manager later
    public void UpdateEventLabel(string eventText)
    {
        event_label.text = "event: " + eventText;
    }

    public void AddCoins(int amount)
    {
        totalCoins += amount;
        UpdateCoinLabel();
    }
}
