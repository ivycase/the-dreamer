using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class Organ : MonoBehaviour
{
    [HeaderAttribute("Animation")]
    public Transform moveTarget;
    public float moveDuration = 0.5f;
    public void UseOrgan()
    {
        print("ni hao");
        transform.DOMove(moveTarget.position, moveDuration).SetEase(Ease.InOutBack);
        transform.DORotate(moveTarget.rotation.eulerAngles, moveDuration).SetEase(Ease.InSine);
    }
}
