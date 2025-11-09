using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine.Experimental.GlobalIllumination;

public class OpenLeftElevator : MonoBehaviour
{
    [SerializeField] private KeyCode triggerKey = KeyCode.P;
    public List<GameObject> doorPrefabs;
    public GameObject roulettePrefab;
    public GameObject wall;
    public Renderer lightTube;
    public int materialIndex = 0;
    public Material newMaterial;
    public Light pointLight;
    public Light spotLight;

    private float doorOpenDistance = 1.2f;
    private float doorOpenDuration = 1.6f;

    private bool hasTriggered = false;

    public SoundManager soundManager;

    void Update()
    {
        if (Input.GetKeyDown(triggerKey) && !hasTriggered)
        {
            hasTriggered = true;
            OpenLeft();
        }
    }

    public void OpenLeft()
    {
        soundManager.PlayOpenDoor();

        if (roulettePrefab != null)
            roulettePrefab.SetActive(true);

        if (doorPrefabs != null && doorPrefabs.Count >= 2)
            {
            doorPrefabs[0].transform.DOLocalMoveX(doorPrefabs[0].transform.localPosition.x + doorOpenDistance, doorOpenDuration).SetEase(Ease.OutBounce);
            doorPrefabs[1].transform.DOLocalMoveX(doorPrefabs[1].transform.localPosition.x - doorOpenDistance, doorOpenDuration).SetEase(Ease.OutBounce); ;
        }

        if (wall != null) wall.SetActive(false);

        if (lightTube != null && newMaterial != null)
        {
            Material[] mats = lightTube.materials;
            mats[materialIndex] = newMaterial;
            lightTube.materials = mats;
        }

        if (pointLight != null) pointLight.enabled = true;
        if (spotLight != null) spotLight.enabled = true;
    }
}