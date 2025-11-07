using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OrganManager : MonoBehaviour
{
    public bool isEnabled;

    public CoinManager coinManager;
    public SoundManager soundManager;
    public int organsUnlocked;
    public int organsOnScale;
    public List<GameObject> organs;

    public GameObject rightScale;
    public GameObject leftScale;
    public GameObject pointer;

    private Vector3 rightScaleStartPos;
    private Vector3 leftScaleStartPos;

    [HeaderAttribute("Animation")]
    public GameObject anubis;
    public Light[] endLights;
    public float endAnimDuration = 10f;

    private bool isEnding;

    private void Start()
    {
        rightScaleStartPos = rightScale.transform.localPosition;
        leftScaleStartPos = leftScale.transform.localPosition;
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

    public void AddToScale()
    {
        organsOnScale += 1;

        AdjustScales();
    }
    
    public void AdjustScales()
    {
        if (!isEnabled) return;

        //int coinWeight = coinManager.totalCoins / 100;
        //int organWeight = organsOnScale * 4;

        float coinWeight = Mathf.Lerp(0f, 1f, coinManager.totalCoins / 2000f);
        float organWeight = Mathf.Lerp(0f, 1f, organsOnScale / 3f);

        Vector3 weighting = new Vector3(0f, -0.5f, 0f);

        //pointer.transform.DORotate(new Vector3(Mathf.Lerp(-110f, -70f, (coinWeight - organWeight)), 180f, -90f), 0.2f).SetEase(Ease.OutBack);
        leftScale.transform.DOLocalMove(weighting * (coinWeight - organWeight) + leftScaleStartPos, 0.2f).SetEase(Ease.OutBack);
        rightScale.transform.DOLocalMove(weighting * (organWeight - coinWeight) + rightScaleStartPos, 0.2f).SetEase(Ease.OutBack);

        if (coinWeight >= organWeight)
        {
            // No win.
            return;
        }

        //coinManager.coin_label.enabled = false;
        if (isEnding) return;
        isEnding = true;
        StartCoroutine(WinAnim());
        return;
    }

    IEnumerator WinAnim()
    {
        foreach(Light light in endLights)
        {
            light.DOIntensity(5000f, endAnimDuration).SetEase(Ease.InBounce);
        }

        soundManager.PlayOpenDoor();
        anubis.transform.DOShakeScale(endAnimDuration * 0.2f, 10f).SetEase(Ease.InExpo);
        yield return new WaitForSeconds(endAnimDuration * 0.2f);
        soundManager.PlayOpenDoor();
        anubis.transform.DOShakeScale(endAnimDuration * 0.2f, 10f).SetEase(Ease.InExpo);
        yield return new WaitForSeconds(endAnimDuration * 0.2f);
        soundManager.PlayOpenDoor();
        anubis.transform.DOShakeScale(endAnimDuration * 0.2f, 10f).SetEase(Ease.InExpo);

        yield return new WaitForSeconds(endAnimDuration * 0.4f);
        SceneManager.LoadScene("BeachScene", LoadSceneMode.Single);
    }
}
