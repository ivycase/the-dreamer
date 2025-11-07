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
    public float maxInteractDistance = 1.0f;

    public string[] chambers;
    public float chamberInterval = 30.0f;

    [HeaderAttribute("Animation")]
    public float spinDuration = 2.0f;
    public float transitionPunchAmount = 1.1f;
    public float transitionPunchDuration = 0.5f;

    [HeaderAttribute("Animateable")]
    public Transform gunCylinder;
    public Transform gun;

    public SoundManager soundManager;

    private int chamberIndex;
    private bool isSpinning;

    void Update()
    {
        if (!isEnabled) return;

        if (Input.GetKeyDown(KeyCode.E) || Input.GetButtonDown("Fire1"))
        {
            if (Vector3.Distance(player.transform.position, gunParent.transform.position) > maxInteractDistance) return;

            //isEnabled = false;
            isSpinning = false;
            coinManager.AddCoins(-coinManager.costPerSpin * 0);
            Stop(0f);
        }
        else if (!isSpinning)
        {
            isSpinning = true;
            soundManager.PlayTick();
            StartCoroutine(Spin(spinDuration));
        }
    }

    private IEnumerator Spin(float duration)
    {
        gunCylinder.transform.DOBlendableLocalRotateBy(new Vector3(0f, 0f, chamberInterval), duration, RotateMode.FastBeyond360);

        chamberIndex = (chamberIndex + 1) % chambers.Length;

        yield return new WaitForSeconds(duration);

        isSpinning = false;
    }

    private void Stop(float duration)
    {
        StopAllCoroutines();
        string outcome = chambers[chamberIndex];
        coinManager.UpdateEventLabel(outcome);
        rewardSystem.ActivateReward(outcome);
    }
}
