using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class OpenLeftElevator : MonoBehaviour
{
    [SerializeField] private KeyCode triggerKey = KeyCode.P;
    public List<GameObject> doorPrefabs;
    public GameObject wall;
    public Renderer lightTube;
    public int materialIndex = 0;
    public Material newMaterial;
    public Light pointLight;

    private float doorOpenDistance = 1.2f;
    private float doorOpenDuration = 0.6f;

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
        if (doorPrefabs != null && doorPrefabs.Count >= 2)
            {
            doorPrefabs[0].transform.DOLocalMoveX(doorPrefabs[0].transform.localPosition.x + doorOpenDistance, doorOpenDuration);
            doorPrefabs[1].transform.DOLocalMoveX(doorPrefabs[1].transform.localPosition.x - doorOpenDistance, doorOpenDuration);
        }

        if (wall != null) wall.SetActive(false);

        if (lightTube != null && newMaterial != null)
        {
            Material[] mats = lightTube.materials;
            mats[materialIndex] = newMaterial;
            lightTube.materials = mats;
        }

        if (pointLight != null) pointLight.enabled = true;
    }
}