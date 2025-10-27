
using UnityEngine;

public class FlickerLight : MonoBehaviour
{
    [SerializeField] private Light targetLight;
    [SerializeField] private float minIntensity = 0.5f;
    [SerializeField] private float maxIntensity = 1.5f;
    [SerializeField] private float flickerSpeed = 10f;
    [SerializeField] private float smoothness = 1f;

    private float noiseOffset;

    void Start()
    {
        if (targetLight == null) targetLight = GetComponent<Light>();
        noiseOffset = Random.Range(0f, 1000f);
    }

    void Update()
    {
        if (targetLight == null) return;

        float noise = Mathf.PerlinNoise(
            Time.time * flickerSpeed * smoothness + noiseOffset,
            noiseOffset
        );

        targetLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, noise);
    }
}