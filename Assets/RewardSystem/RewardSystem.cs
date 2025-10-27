using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class RewardSystem : MonoBehaviour
{
    [HeaderAttribute("Coin")]
    public CoinManager coinManager;
    public int coinReward;
    public GameObject coinParticleParent;
    public GameObject r_coinParticleParent;

    [HeaderAttribute("Butterfly")]
    public ParticleSystem butterflyParticles;

    [HeaderAttribute("Music")]
    public List<AudioSource> musicSources;

    [HeaderAttribute("Cards")]
    public GameObject cardsParent;
    public float cardsRotateDuration = 2.0f;

    [HeaderAttribute("Balls")]
    public GameObject regularBall;
    public GameObject appleBall;
    public GameObject eyeBall;
    public GameObject skullBall;
    public GameObject saturnBall;

    [HeaderAttribute("Size")]
    public GameObject rouletteTable;
    public float tableGrowDuration = 0.25f;
    public float[] tableSizes;

    public GameObject currentBall;
    private int musicIndex = 0;

    private int tableSizeIndex;

    public void Start()
    {
        currentBall = regularBall;
    }

    public void ActivateReward(string reward_name)
    {
        coinManager.UpdateEventLabel(reward_name);

        switch (reward_name)
        {
            case "loss":
                //TODO: money loss effects
                break;


            // ** SLOT MACHINE **
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
                musicSources[musicIndex].Pause();
                musicIndex = (musicIndex + 1) % musicSources.Count;
                AudioSource source = musicSources[musicIndex];
                source.Play();
                break;

            case "cards":
                foreach (Transform card in cardsParent.GetComponentsInChildren<Transform>())
                {
                    card.DORotate(new Vector3(0, 0, 180), cardsRotateDuration, RotateMode.LocalAxisAdd);
                }
                break;

            // ** ROULETTE **
            case "r_coin":
                coinManager.AddCoins(coinReward);

                currentBall.SetActive(false);
                regularBall.SetActive(true);
                currentBall = regularBall;

                r_coinParticleParent.SetActive(true);
                foreach (ParticleSystem particle in r_coinParticleParent.GetComponentsInChildren<ParticleSystem>())
                {
                    particle.Play();
                }
                break;

            case "apple":
                currentBall.SetActive(false);
                appleBall.SetActive(true);
                currentBall = appleBall;
                break;

            case "eye":
                currentBall.SetActive(false);
                eyeBall.SetActive(true);
                currentBall = eyeBall;
                break;

            case "skull":
                currentBall.SetActive(false);
                skullBall.SetActive(true);
                currentBall = skullBall;
                break;

            case "saturn":
                currentBall.SetActive(false);
                saturnBall.SetActive(true);
                currentBall = saturnBall;
                break;

            case "size":
                if (tableSizeIndex >= tableSizes.Length) return;

                rouletteTable.transform.DOBlendableScaleBy(Vector3.one * tableSizes[tableSizeIndex], tableGrowDuration);
                tableSizeIndex++;

                break;
        }
    }
}
