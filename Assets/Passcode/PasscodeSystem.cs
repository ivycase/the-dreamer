using NUnit.Framework;
using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PasscodeSystem : MonoBehaviour
{
    public RewardSystem rewardSystem;

    public string correctSignal = "correct";

    public List<string> correctSequence = new();

    private List<string> currentSequence = new();

    public void AddToSequence(CodeButton button)
    {
        if (currentSequence.Contains(button.value)) return;

        if (button.TryGetComponent(out Light light))
        {
            light.enabled = true;
        }

        currentSequence.Add(button.value);
        //print(currentSequence.Count);
        if (currentSequence.Count >= correctSequence.Count)
        {
            print(currentSequence[0] + " " + currentSequence[1] + " " + currentSequence[2]);
            print(correctSequence[0] + " " + correctSequence[1] + " " + correctSequence[2]);
            CheckSequence(currentSequence);

            currentSequence.Clear();

            foreach (Light lgt in button.transform.parent.parent.GetComponentsInChildren<Light>())
            {
                StartCoroutine(DelayOff(lgt, 0.2f));
            }

        }
    }

    void CheckSequence(List<string> sequence)
    {
        for (int i = 0; i < sequence.Count; i++) {
            if (sequence[i] != correctSequence[i])
            {
                rewardSystem.ActivateReward("incorrect");
                return;
            }
        }

        rewardSystem.ActivateReward(correctSignal);
    }

    IEnumerator DelayOff(Light target, float delay)
    {
        yield return new WaitForSeconds(delay);
        target.enabled = false;
    }


}
