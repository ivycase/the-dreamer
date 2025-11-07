using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
public class OpenFrontElevator : MonoBehaviour
{
    [SerializeField] private KeyCode triggerKey = KeyCode.M;
    public GameObject movePrefab;
    public List<GameObject> doorPrefabs;

    public float moveDistance = 1f;
    public float moveDuration = 1f;
    private float doorOpenDistance = 1.2f;
    private float doorOpenDuration = 1.6f;
    public float moveDelay = 0.5f;

    public bool hasTriggered = false;

    public SoundManager soundManager;
    void Update()
    {
        if (Input.GetKeyDown(triggerKey) && !hasTriggered)
        {
            OpenFront();
        }
    }
    public void OpenFront()
    {
        hasTriggered = true;
        soundManager.PlayOpenDoor();
        if (doorPrefabs != null && doorPrefabs.Count >= 2)
        {
            doorPrefabs[0].transform.DOLocalMoveX(doorPrefabs[0].transform.localPosition.x + doorOpenDistance, doorOpenDuration).SetEase(Ease.OutBounce);
            doorPrefabs[1].transform.DOLocalMoveX(doorPrefabs[1].transform.localPosition.x - doorOpenDistance, doorOpenDuration).SetEase(Ease.OutBounce);
        }
        if (movePrefab != null)
        {
            movePrefab.transform.DOLocalMoveX(movePrefab.transform.localPosition.x + moveDistance, moveDuration)
                .SetDelay(moveDelay);
        }

    }
}