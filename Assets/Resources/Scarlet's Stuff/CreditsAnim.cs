using UnityEngine;
using UnityEngine.UI;

public class CreditsAnim : MonoBehaviour
{
    public Image targetImage;
    public Vector2 startPosition;
    public Vector2 endPosition;
    public float duration = 1f;

    private float timer = 0f;

    void Start()
    {
        targetImage.rectTransform.anchoredPosition = startPosition;
    }

    void Update()
    {
        timer += Time.deltaTime;
        float t = Mathf.Clamp01(timer / duration);
        targetImage.rectTransform.anchoredPosition = Vector2.Lerp(startPosition, endPosition, t);
    }
}