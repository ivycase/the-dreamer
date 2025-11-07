using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource ScreamSource;
    public AudioSource ButterSource;
    public AudioSource CoinSource;
    public AudioSource CardSource;
    public AudioSource ButtonSource;
    public AudioSource ShootSource;
    public AudioSource TickSource;
    public AudioSource SpinSource;
    public AudioSource ClickSource;
    public AudioSource OpenDoorSource;

    public void PlayScream()
    {
        if (ScreamSource != null) ScreamSource.Play();
    }

    public void PlayButterfly()
    {
        if (ButterSource != null) ButterSource.Play();
    }
    public void PlayCoin()
    {
        if (CoinSource != null) CoinSource.Play();
    }

    public void PlayCard()
    {
        if (CardSource != null) CardSource.Play();
    }

    public void PlayButton()
    {
        ButtonSource.volume = 10f;
        if (ButtonSource != null) ButtonSource.Play();
    }

    public void PlayShoot()
    {
        if (ShootSource != null) ShootSource.Play();
    }

    public void PlayTick()
    {
        if (TickSource != null) TickSource.Play();
    }

    public void PlaySpin() 
    {
        if (SpinSource != null) SpinSource.Play();
    }

    public void PlayClick()
    {
        if (ClickSource != null) ClickSource.Play();
    }

    public void PlayOpenDoor() 
    {
        if (OpenDoorSource != null) OpenDoorSource.Play();
    }
}