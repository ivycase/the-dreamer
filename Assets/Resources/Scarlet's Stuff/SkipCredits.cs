using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;
public class SkipCredits : MonoBehaviour
{
    [SerializeField] private float skipDelay = 15f;
    [SerializeField] private TextMeshProUGUI skipText;
    private bool canSkip = false;
    public int SkipSceneIndex = 3;
    void Start()
    {
        Invoke("EnableSkip", skipDelay);
        skipText.text = "";
    }
    void Update()
    {
        if (canSkip && (Keyboard.current.anyKey.wasPressedThisFrame ||
                       Mouse.current.leftButton.wasPressedThisFrame ||
                       Mouse.current.rightButton.wasPressedThisFrame))
        {
            SkipVideo();
        }
    }
    private void EnableSkip()
    {
        canSkip = true;
        skipText.text = "Press any key to skip...";
    }
    private void SkipVideo()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - SkipSceneIndex);
    }
}