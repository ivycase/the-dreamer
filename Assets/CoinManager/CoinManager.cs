using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class CoinManager : MonoBehaviour
{
    public TMP_Text coin_label;
    public TMP_Text event_label;
    public TMP_Text gameOverText;
    public Image redBackground;
    public PostProcessVolume postProcessVolume;
    public int totalCoins;
    public int costPerSpin;
    public Color positive_top, positive_down, negative_top, negative_down = new Color(1f, 1f, 1f);

    private ColorGrading colorGrading;

    private void Start()
    {
        if (gameOverText != null) gameOverText.gameObject.SetActive(false);
        if (redBackground != null) redBackground.gameObject.SetActive(false);

        if (postProcessVolume != null && postProcessVolume.profile.TryGetSettings(out colorGrading))
        {
            colorGrading.colorFilter.value = Color.white;
            colorGrading.colorFilter.overrideState = true;
            colorGrading.postExposure.value = 0f;
            colorGrading.postExposure.overrideState = true;
        }

        UpdateCoinLabel();
    }

    private void UpdateCoinLabel()
    {
        coin_label.text = "$" + totalCoins;
        coin_label.enableVertexGradient = true;

        if (totalCoins < 0)
        {
            coin_label.colorGradient = new VertexGradient(negative_top, negative_top, negative_down, negative_down);
            UpdateRedTint();
        }
        else
        {
            coin_label.colorGradient = new VertexGradient(positive_top, positive_top, positive_down, positive_down);
            ResetEffects();
        }

        if (totalCoins <= -1000)
        {
            GameOver();
        }
    }

    private void UpdateRedTint()
    {
        float tintAmount = Mathf.Clamp01(Mathf.Abs(totalCoins) / 1000f);

        if (colorGrading != null)
        {
            Color redTint = Color.Lerp(Color.white, new Color(20f, 0f, 0f), tintAmount);
            colorGrading.colorFilter.value = redTint;
            colorGrading.postExposure.value = Mathf.Lerp(0f, -3.5f, tintAmount);
        }

        if (redBackground != null)
        {
            redBackground.gameObject.SetActive(true);
            float bloodAmount = Mathf.Clamp01((Mathf.Abs(totalCoins) - 500f) / 500f);
            Color blood = redBackground.color;
            blood.a = bloodAmount;
            redBackground.color = blood;
        }
    }

    private void ResetEffects()
    {
        if (colorGrading != null)
        {
            colorGrading.colorFilter.value = Color.white;
            colorGrading.postExposure.value = 0f;
        }

        if (redBackground != null)
        {
            Color blood = redBackground.color;
            blood.a = 0f;
            redBackground.color = blood;
        }
    }

    private void GameOver()
    {
        if (colorGrading != null)
        {
            colorGrading.colorFilter.value = new Color(20f, 0f, 0f);
            colorGrading.postExposure.value = -3.5f;
        }

        if (redBackground != null)
        {
            Color blood = redBackground.color;
            blood.a = 1f;
            redBackground.color = blood;
        }

        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(true);
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