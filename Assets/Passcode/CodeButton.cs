using UnityEngine;

public class CodeButton : MonoBehaviour
{
    public PasscodeSystem passcodeSystem;

    public int value;
    public void Press()
    {
        passcodeSystem.AddToSequence(this);
        //if there is an audio source attached, play it
        if (TryGetComponent(out AudioSource audio))
        {
            audio.pitch = Random.Range(.95f, 1.1f);
            audio.volume = Random.Range(.8f, 1.2f);
            audio.Play();
        }
    }
}
