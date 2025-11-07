using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class Organ : MonoBehaviour
{
    public OrganManager organManager;

    [HeaderAttribute("Animation")]
    public Transform moveTarget;
    public float moveDuration = 0.5f;
    public void UseOrgan()
    {
        if (!organManager.isEnabled) return;

        GetComponent<Collider>().enabled = false;
        transform.SetParent(moveTarget);
        transform.DOLocalMove(Vector3.zero, moveDuration).SetEase(Ease.InOutBack);
        transform.DOLocalRotate(Vector3.zero, moveDuration).SetEase(Ease.InSine);
        organManager.AddToScale();
    }
}
