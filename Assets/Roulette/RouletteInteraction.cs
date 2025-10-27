using DG.Tweening;
using System.Collections;
using UnityEngine;

public class RouletteInteraction : MonoBehaviour
{
    public bool isEnabled;

    public RewardSystem rewardSystem;
    public CoinManager coinManager;

    public GameObject player;
    public GameObject rouletteTable;
    public float maxInteractDistance = 5.0f;

    public string[] symbols;
    public float[] symbolRotations;

    [HeaderAttribute("Animation")]
    public float ballSpinDuration = 2.0f;

    [HeaderAttribute("Animateable")]
    public Transform ballPivot;
    public Transform ball;

    private int symbolIndex; // symbol order is fixed on the roulette table
    private bool isSpinning;

    void Update()
    {
        if (isSpinning || !isEnabled) return;

        if (Input.GetKeyDown(KeyCode.E) || Input.GetButtonDown("Fire1"))
        {
            if (Vector3.Distance(player.transform.position, rouletteTable.transform.position) > maxInteractDistance) return;

            isSpinning = true;
            coinManager.AddCoins(-coinManager.costPerSpin);
            StartCoroutine(Spin(ballSpinDuration));
        }
    }

    private IEnumerator Spin(float duration)
    {
        string targetSymbol = symbols[symbolIndex];

        Vector3 targetRotation = ballPivot.transform.localRotation.eulerAngles;
        targetRotation.y = symbolRotations[symbolIndex] + 360.0f * (int)duration;

        ballPivot.transform.DORotate(targetRotation, duration, RotateMode.FastBeyond360).SetEase(Ease.OutSine);
        ball.transform.DORotate(targetRotation, duration, RotateMode.FastBeyond360).SetEase(Ease.OutSine);

        yield return new WaitForSeconds(duration);

        coinManager.UpdateEventLabel(targetSymbol);
        rewardSystem.ActivateReward(targetSymbol);
        symbolIndex = (symbolIndex + 1) % symbols.Length;
        isSpinning = false;
    }
}
