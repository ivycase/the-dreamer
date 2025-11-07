using DG.Tweening;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OrganManager : MonoBehaviour
{
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
        SceneManager.LoadScene("BeachScene", LoadSceneMode.Single);
        return;
    }
}
