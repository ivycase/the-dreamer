using UnityEngine;

public class CodeButton : MonoBehaviour
{
    public PasscodeSystem passcodeSystem;
    public SoundManager soundManager;

    public string value;
    public void Press()
    {
        passcodeSystem.AddToSequence(this);
        //if there is an audio source attached, play it
        soundManager.PlayButton();
    }
}
