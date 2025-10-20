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

    public float biasAmount = 0.2f;
    public string[] symbols;
    public float[] symbolRotations;

    // maybe put this in an event manager !
    [HeaderAttribute("Rewards")]
    public int coinReward;
    public GameObject coinParticleParent;
    public ParticleSystem butterflyParticles;
    public AudioSource musicSource;

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
    private float musicNextPitch = 1.0f;

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

            int biasIndex = -1;

            // sometimes you get a free match to make the game go faster
            if (Random.Range(0f, 1f) <= biasAmount)
            {
                Debug.Log("free match!");
                biasIndex = Random.Range(0, symbols.Length);
            }

            StartCoroutine(Spin(leftRoller, rollerRotateDuration, biasIndex));
            StartCoroutine(Spin(centerRoller, rollerRotateDuration + rollerSpinDelay, biasIndex));
            StartCoroutine(Spin(rightRoller, rollerRotateDuration + rollerSpinDelay * 2.0f, biasIndex, true));
        }
    }

    private IEnumerator Spin(Transform roller, float duration, int biasIndex = -1, bool doEvaluate = false)
    {
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
        targetRotation.z = symbolRotations[symbolIndex] + 360.0f * (int) duration;

        roller.transform.DOLocalRotate(targetRotation, duration, RotateMode.FastBeyond360);

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
        coinManager.UpdateEventLabel(left);

        switch (left)
        {
            case "coin":
                coinManager.AddCoins(coinReward);

                coinParticleParent.SetActive(true);
                foreach (ParticleSystem particle in coinParticleParent.GetComponentsInChildren<ParticleSystem>())
                {
                    particle.Play();
                }
                break;

            case "butterfly":
                butterflyParticles.gameObject.SetActive(true);
                butterflyParticles.Play();
                break;

            case "music":
                musicSource.pitch = musicNextPitch;
                musicSource.Play();
                musicNextPitch = Random.Range(0.75f, 1.25f);
                break;

            case "gem":
                break;
        }
    }
}
