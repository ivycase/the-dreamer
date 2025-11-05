using DG.Tweening;
using UnityEngine;
using System.Collections;

public class RussianRouletteInteraction : MonoBehaviour
{
    public bool isEnabled;

    public RewardSystem rewardSystem;
    public CoinManager coinManager;

    public GameObject player;
    public GameObject gunParent;
    public float maxInteractDistance = 5.0f;

    public string[] chambers;
    public float[] chamberRotations;

    [HeaderAttribute("Animation")]
    public float spinDuration = 2.0f;
    public float transitionPunchAmount = 1.1f;
    public float transitionPunchDuration = 0.5f;

    [HeaderAttribute("Animateable")]
    public Transform gunCylinder;
    public Transform gun;

    private int chamberIndex;
    private bool isSpinning;

    void Update()
    {
        if (!isEnabled) return;

        if (Input.GetKeyDown(KeyCode.E) || Input.GetButtonDown("Fire1"))
        {
            if (Vector3.Distance(player.transform.position, gunParent.transform.position) > maxInteractDistance) return;

            isSpinning = true;
            coinManager.AddCoins(-coinManager.costPerSpin * 0);
            StartCoroutine(Spin(spinDuration));
        }
        /*else if (!isSpinning)
        {
            isSpinning = true;
            StartCoroutine(Spin(spinDuration));
        }*/
    }

    private IEnumerator Spin(float duration)
    {
        string targetSymbol = chambers[chamberIndex];

        Vector3 targetRotation = gunCylinder.transform.localRotation.eulerAngles;
        targetRotation.y = chamberRotations[chamberIndex] + 360.0f * (int)duration;

        gunCylinder.transform.DORotate(targetRotation, duration, RotateMode.FastBeyond360).SetEase(Ease.OutSine);

        coinManager.UpdateEventLabel(targetSymbol);
        chamberIndex = (chamberIndex + 1) % chambers.Length;

        yield return new WaitForSeconds(duration);

        isSpinning = false;
    }

    private IEnumerator Stop(float duration)
    {
        gunCylinder.transform.DOComplete();
        yield return new WaitForSeconds(duration);
        isSpinning = false;
    }
}
