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
    private float doorOpenDuration = 0.6f;
    public float moveDelay = 0.5f;

    private bool hasTriggered = false;
    void Update()
    {
        if (Input.GetKeyDown(triggerKey) && !hasTriggered)
        {
            hasTriggered = true;
            OpenFront();
        }
    }
    public void OpenFront()
    {
        if (doorPrefabs != null && doorPrefabs.Count >= 2)
        {
            doorPrefabs[0].transform.DOLocalMoveX(doorPrefabs[0].transform.localPosition.x + doorOpenDistance, doorOpenDuration);
            doorPrefabs[1].transform.DOLocalMoveX(doorPrefabs[1].transform.localPosition.x - doorOpenDistance, doorOpenDuration);
        }
        if (movePrefab != null)
        {
            movePrefab.transform.DOLocalMoveX(movePrefab.transform.localPosition.x + moveDistance, moveDuration)
                .SetDelay(moveDelay);
        }

    }
}