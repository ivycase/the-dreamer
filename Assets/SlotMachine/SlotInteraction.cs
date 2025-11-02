using DG.Tweening;

using OldElevator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SlotInteraction : MonoBehaviour
{
    public RewardSystem rewardSystem;
    public CoinManager coinManager;
    public GameObject player;
    public GameObject slotMachine;
    public float maxInteractDistance = 5.0f;
    public float biasAmount = 0.2f;
    public string[] symbols;
    public float[] symbolRotations;

    [HeaderAttribute("Animation")]
    public float slotShakeAmplitude = 1.0f;
    public float leverRotateDuration = 0.5f;
    public float rollerRotateDuration = 0.6f;
    public float rollerSpinDelay = 0.6f;

    [HeaderAttribute("Audio")]
    public AudioSource leftRollerAudio;
    public AudioSource centerRollerAudio;
    public AudioSource rightRollerAudio;
    public AudioSource leverAudioSource;
    public AudioClip spinSound;
    public AudioClip leverSound;
    public float audioStopOffset = 0.1f;

    [HeaderAttribute("Animateable")]
    public Transform lever;
    public Transform leftRoller;
    public Transform centerRoller;
    public Transform rightRoller;

    private bool isSpinning;
    private bool isFirstSpin = true;
    private Dictionary<Transform, string> rollerSymbols = new();

    public void Update()
    {
        if (isSpinning) return;

        if (Input.GetKeyDown(KeyCode.E) || Input.GetButtonDown("Fire1"))
        {
            if (Vector3.Distance(player.transform.position, slotMachine.transform.position) > maxInteractDistance) return;

            isSpinning = true;
            coinManager.AddCoins(-coinManager.costPerSpin);

            if (leverAudioSource != null && leverSound != null)
            {
                leverAudioSource.PlayOneShot(leverSound);
            }

            slotMachine.transform.DOShakePosition(leverRotateDuration / 2.0f, slotShakeAmplitude, 25);
            lever.DOBlendablePunchRotation(new Vector3(0f, 0f, 60f), leverRotateDuration, 1, 0.1f);

            int biasIndex = -1;

            if (isFirstSpin)
            {
                for (int i = 0; i < symbols.Length; i++)
                {
                    if (symbols[i] == "music")
                    {
                        biasIndex = i;
                        break;
                    }
                }
                isFirstSpin = false;
            }
            else if (Random.Range(0f, 1f) <= biasAmount)
            {
                Debug.Log("free match!");
                biasIndex = Random.Range(0, symbols.Length);
            }

            StartCoroutine(Spin(leftRoller, rollerRotateDuration, leftRollerAudio, biasIndex));
            StartCoroutine(Spin(centerRoller, rollerRotateDuration + rollerSpinDelay, centerRollerAudio, biasIndex));
            StartCoroutine(Spin(rightRoller, rollerRotateDuration + rollerSpinDelay * 2.0f, rightRollerAudio, biasIndex, true));
        }
    }

    private IEnumerator Spin(Transform roller, float duration, AudioSource audioSource, int biasIndex = -1, bool doEvaluate = false)
    {
        if (audioSource != null && spinSound != null)
        {
            audioSource.clip = spinSound;
            audioSource.loop = true;
            audioSource.Play();
        }

        int symbolIndex;
        if (biasIndex == -1)
        {
            symbolIndex = Random.Range(0, symbols.Length);
        }
        else
        {
            symbolIndex = biasIndex;
        }

        string targetSymbol = symbols[symbolIndex];
        rollerSymbols[roller] = targetSymbol;

        Vector3 targetRotation = roller.transform.localRotation.eulerAngles;
        targetRotation.z = symbolRotations[symbolIndex] + 360.0f * (int)duration;
        roller.transform.DOLocalRotate(targetRotation, duration, RotateMode.FastBeyond360);

        yield return new WaitForSeconds(duration - audioStopOffset);

        if (audioSource != null)
        {
            audioSource.loop = false;
            audioSource.Stop();
        }

        yield return new WaitForSeconds(audioStopOffset);

        if (!doEvaluate) yield break;

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
            rewardSystem.ActivateReward("loss");
            return;
        }

        rewardSystem.ActivateReward(left);
    }
}