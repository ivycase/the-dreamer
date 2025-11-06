using UnityEngine;

public class CodeButton : MonoBehaviour
{
    public PasscodeSystem passcodeSystem;
    public SoundManager soundManager;

    public int value;
    public void Press()
    {
        passcodeSystem.AddToSequence(this);
        //if there is an audio source attached, play it
        soundManager.PlayButton();
    }
}
