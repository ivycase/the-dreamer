using DG.Tweening;
using OldElevator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotInteraction : MonoBehaviour
{
    public GameObject player;
    public GameObject slotMachine;
    public float maxInteractDistance = 5.0f;

    public string[] symbols;
    public float[] symbolRotations;

    [HeaderAttribute("Animation")]
    public float leverRotateDuration = 0.5f;
    public float rollerRotateDuration = 1.0f;
    public float rollerSpinDelay = 1.0f;

    [HeaderAttribute("Animateable")]
    public Transform lever;
    public Transform leftRoller;
    public Transform centerRoller;
    public Transform rightRoller;
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) || Input.GetButtonDown("Fire1"))
        {
            if (Vector3.Distance(player.transform.position, slotMachine.transform.position) > maxInteractDistance) return;

            lever.DOPunchRotation(new Vector3(60f, 0f, 0f), leverRotateDuration, 1, 0.1f);

            Spin(leftRoller, rollerRotateDuration);
            Spin(centerRoller, rollerRotateDuration + rollerSpinDelay);
            Spin(rightRoller, rollerRotateDuration + rollerSpinDelay * 2.0f);
        }
    }

    private void Spin(Transform roller, float duration)
    {
        int symbol_index = Random.Range(0, symbols.Length);

        string target_symbol = symbols[symbol_index];

        Vector3 target_rotation = roller.transform.localRotation.eulerAngles;
        target_rotation.z = symbolRotations[symbol_index] + 360.0f * (int) duration;

        roller.transform.DOLocalRotate(target_rotation, duration, RotateMode.FastBeyond360);
    }
}
