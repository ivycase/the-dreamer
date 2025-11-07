using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class OrganManager : MonoBehaviour
{
    public SoundManager soundManager;
    public int organsUnlocked;
    public int organsOnScale;
    public List<GameObject> organs;

    public void AddOrgan()
    {
        if (organsUnlocked >= organs.Count) return;

        organs[organsUnlocked].SetActive(true);
        organs[organsUnlocked].transform.DOPunchScale(new Vector3(1.1f, 1.1f, 1.1f), 0.25f);
        organs[organsUnlocked].transform.DOPunchRotation(new Vector3(0f, 60f, 0f), 0.2f);
        soundManager.PlaySquish();
        organsUnlocked += 1;
    }
}
