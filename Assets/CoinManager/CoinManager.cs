using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
using System.Collections;
using DG.Tweening;
using NUnit.Framework;
using System.Collections.Generic;


public class CoinManager : MonoBehaviour
{
    public SoundManager soundManager;
    public TMP_Text coin_label;
    public TMP_Text event_label;
    public TMP_Text gameOverText;
    public Image redBackground;
    public PostProcessVolume postProcessVolume;
    public int totalCoins;
    public int costPerSpin;
    public Color positive_top, positive_down, negative_top, negative_down = new Color(1f, 1f, 1f);
    public MonoBehaviour playerController;
    public Transform respawnPoint;
    private bool resetMoney = false;
    public bool isDead = false;

    [HeaderAttribute("Organs")]
    public int organsUnlocked;
    public int organsOnScale;
    public List<GameObject> organs;

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

        if (totalCoins < 500)
        {
            coin_label.colorGradient = new VertexGradient(negative_top, negative_top, negative_down, negative_down);
            UpdateRedTint();
        }
        else
        {
            coin_label.colorGradient = new VertexGradient(positive_top, positive_top, positive_down, positive_down);
            ResetEffects();
        }

        if (totalCoins <= 0)
        {
            GameOver();
        }
    }

    private void UpdateRedTint()
    {
        float tintAmount = Mathf.Clamp01(Mathf.Abs(600f - totalCoins) / 1000f);

        if (colorGrading != null)
        {
            Color redTint = Color.Lerp(Color.white, new Color(1f, 0f, 0f), tintAmount);
            colorGrading.colorFilter.value = redTint;
            colorGrading.postExposure.value = Mathf.Lerp(0f, -0.6f, tintAmount);
        }

        //if (redBackground != null)
        //{
        //    redBackground.gameObject.SetActive(true);
        //    float bloodAmount = Mathf.Clamp01((Mathf.Abs(totalCoins) - 500f) / 500f);
        //    Color blood = redBackground.color;
        //    blood.a = bloodAmount;
        //    redBackground.color = blood;
        //}
    }

    private void ResetEffects()
    {
        if (colorGrading != null)
        {
            colorGrading.colorFilter.value = Color.white;
            colorGrading.postExposure.value = 0f;
        }

        //if (redBackground != null)
        //{
        //    Color blood = redBackground.color;
        //    blood.a = 0f;
        //    redBackground.color = blood;
        //}
    }
    public void GameOver()
    {
        resetMoney = true;
        if (playerController != null) playerController.enabled = false;
        if (colorGrading != null)
        {
            colorGrading.colorFilter.value = new Color(20f, 0f, 0f);
        }
        //if (redBackground != null)
        //{
        //    Color blood = redBackground.color;
        //    blood.a = 1f;
        //    redBackground.color = blood;
        //}
        StartCoroutine(Respawn());
    }

    public void Gunshot()
    {
        resetMoney = false;
        soundManager.PlayShoot();
        if (colorGrading != null)
        {
            colorGrading.postExposure.value = 5f;
            DOTween.Sequence()
                .Append(DOTween.To(() => colorGrading.postExposure.value, x => colorGrading.postExposure.value = x, -3.5f, 0.3f));
        }
        Camera.main.transform.DOShakePosition(0.3f, 0.5f, 20);

        if (playerController != null) playerController.enabled = false;
        if (colorGrading != null)
        {
            colorGrading.colorFilter.value = new Color(20f, 0f, 0f);
        }
        if (redBackground != null)
        {
            Color blood = redBackground.color;
            blood.a = 1f;
            redBackground.color = blood;
        }
        StartCoroutine(Respawn());
    }

    IEnumerator Respawn()
    {
        isDead = true;
        yield return new WaitForSeconds(0.4f);
        float t = 0;
        soundManager.PlayScream();
        while (t < 1f)
        {
            t += Time.deltaTime;
            colorGrading.postExposure.value = Mathf.Lerp(-3.5f, -15f, t / 1f);
            yield return null;
        }

        if (respawnPoint != null && playerController != null)
        {
            playerController.transform.position = respawnPoint.position;
            if (resetMoney == false) playerController.transform.rotation = respawnPoint.rotation;
            if (resetMoney == true) playerController.transform.rotation = respawnPoint.rotation * Quaternion.Euler(0, 180, 0);
        }

        yield return new WaitForSeconds(1.5f);
        colorGrading.colorFilter.value = Color.white;
        t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime;
            colorGrading.postExposure.value = Mathf.Lerp(-15f, 0f, t / 1f);
            yield return null;
        }
        if (redBackground != null) redBackground.color = new Color(1, 0, 0, 0);
        if (playerController != null) playerController.enabled = true;
        if (resetMoney == true) 
        {
            totalCoins = 2000;
            soundManager.PlayCoin(); ;
        } 
        UpdateCoinLabel();
        AddOrgan();
        isDead = false;
    }

    public void AddOrgan()
    {
        if (organsUnlocked >= organs.Count) return;

        organs[organsUnlocked].SetActive(true);
        organs[organsUnlocked].transform.DOPunchScale(new Vector3(1.1f, 1.1f, 1.1f), 0.25f);
        organs[organsUnlocked].transform.DOPunchRotation(new Vector3(0f, 60f, 0f), 0.2f);
        soundManager.PlaySquish();
        organsUnlocked += 1;
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