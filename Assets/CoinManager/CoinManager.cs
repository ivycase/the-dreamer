using TMPro;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public TMP_Text coin_label;
    public TMP_Text event_label;
    public int totalCoins;
    public int costPerSpin;

    public Color positive_top = new Color(1f, 0.9f, 0f);
    public Color positive_down = new Color(1f, 0.6f, 0f);
    public Color negative_top = new Color(1f, 0.3f, 0.3f);
    public Color negative_down = new Color(0.5f, 0f, 0f);


    private void Start()
    {
        UpdateCoinLabel();
    }

    private void UpdateCoinLabel()
    {
        coin_label.text = "$" + totalCoins;

        coin_label.enableVertexGradient = true;

        if (totalCoins < 0)
        {
            coin_label.colorGradient = new VertexGradient(negative_top, negative_top, negative_down, negative_down);
        }
        else
        {
            coin_label.colorGradient = new VertexGradient(positive_top, positive_top, positive_down, positive_down);
        }
    }

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