using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class RewardSystem : MonoBehaviour
{
    [HeaderAttribute("Coin")]
    public CoinManager coinManager;
    public int coinReward;
    public GameObject coinParticleParent;

    [HeaderAttribute("Butterfly")]
    public ParticleSystem butterflyParticles;

    [HeaderAttribute("Music")]
    public List<AudioSource> musicSources;

    [HeaderAttribute("Cards")]
    public GameObject cardsParent;
    public float cardsRotateDuration = 2.0f;

    private int musicIndex = 0;

    public void ActivateReward(string reward_name)
    {
        coinManager.UpdateEventLabel(reward_name);

        switch (reward_name)
        {
            case "loss":
                //TODO: money loss effects
                break;

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
        }
    }
}
