using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class OpenRightElevator : MonoBehaviour
{
    [SerializeField] private KeyCode triggerKey = KeyCode.L;
    public GameObject gunPrefab;
    public Light targetLight;
    public List<GameObject> doorPrefabs;

    private float doorOpenDistance = 1.2f;
    private float doorOpenDuration = 0.6f;

    private bool hasTriggered = false;

    public SoundManager soundManager;

    void Update()
    {
        if (Input.GetKeyDown(triggerKey) && !hasTriggered)
        {
            hasTriggered = true;
            OpenRight();
        }
    }

    public void OpenRight()
    {
         soundManager.PlayOpenDoor();
         if (gunPrefab != null)
             gunPrefab.SetActive(true);

         if (targetLight != null)
             targetLight.enabled = true;

         if(doorPrefabs != null && doorPrefabs.Count >= 2)
         {
             doorPrefabs[0].transform.DOLocalMoveX(doorPrefabs[0].transform.localPosition.x + doorOpenDistance, doorOpenDuration);
             doorPrefabs[1].transform.DOLocalMoveX(doorPrefabs[1].transform.localPosition.x - doorOpenDistance, doorOpenDuration);
         }
    }
}
