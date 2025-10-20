using DG.Tweening;
using OldElevator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotInteraction : MonoBehaviour
{
    public CoinManager coinManager;

    public GameObject player;
    public GameObject slotMachine;
    public float maxInteractDistance = 5.0f;

    public string[] symbols;
    public float[] symbolRotations;

    [HeaderAttribute("Rewards")]
    public int coinReward;

    [HeaderAttribute("Animation")]
    public float slotShakeAmplitude = 1.0f;
    public float leverRotateDuration = 0.5f;
    public float rollerRotateDuration = 1.0f;
    public float rollerSpinDelay = 1.0f;

    [HeaderAttribute("Animateable")]
    public Transform lever;
    public Transform leftRoller;
    public Transform centerRoller;
    public Transform rightRoller;

    private bool isSpinning;
    private Dictionary<Transform, string> rollerSymbols = new();

    public void Update()
    {
        if (isSpinning) return;

        if (Input.GetKeyDown(KeyCode.E) || Input.GetButtonDown("Fire1"))
        {
            if (Vector3.Distance(player.transform.position, slotMachine.transform.position) > maxInteractDistance) return;

            isSpinning = true;
            coinManager.AddCoins(-coinManager.costPerSpin);

            slotMachine.transform.DOShakePosition(leverRotateDuration / 2.0f, slotShakeAmplitude, 25);

            lever.DOBlendablePunchRotation(new Vector3(0f, 0f, 60f), leverRotateDuration, 1, 0.1f);

            StartCoroutine(Spin(leftRoller, rollerRotateDuration));
            StartCoroutine(Spin(centerRoller, rollerRotateDuration + rollerSpinDelay));
            StartCoroutine(Spin(rightRoller, rollerRotateDuration + rollerSpinDelay * 2.0f, true));
        }
    }

    private IEnumerator Spin(Transform roller, float duration, bool doEvaluate = false)
    {
        int symbol_index = Random.Range(0, symbols.Length);

        string target_symbol = symbols[symbol_index];
        rollerSymbols[roller] = target_symbol;

        Vector3 target_rotation = roller.transform.localRotation.eulerAngles;
        target_rotation.z = symbolRotations[symbol_index] + 360.0f * (int) duration;

        roller.transform.DOLocalRotate(target_rotation, duration, RotateMode.FastBeyond360);

        if (!doEvaluate) yield break;

        yield return new WaitForSeconds(duration);

        EvaluateResult();
        isSpinning = false;
    }

    private void EvaluateResult()
    {
        string left = rollerSymbols[leftRoller];
        string center = rollerSymbols[centerRoller];
        string right = rollerSymbols[rightRoller];

        if (left != center || left != right || center != right)
        {
            // no match
            return;
        }

        Debug.Log(left + " match!");

        switch (left)
        {
            case "coin":
                coinManager.AddCoins(coinReward);
                break;

            case "butterfly":
                break;

            case "music":
                break;

            case "gem":
                break;
        }
    }
}
