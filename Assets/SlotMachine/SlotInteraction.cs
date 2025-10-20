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
    public float rollerRotateDuration = 1.0f;
    public float rollerSpinDelay = 1.0f;

    [HeaderAttribute("Animateable")]
    public Transform leftRoller;
    public Transform centerRoller;
    public Transform rightRoller;
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) || Input.GetButtonDown("Fire1"))
        {
            if (Vector3.Distance(player.transform.position, slotMachine.transform.position) > maxInteractDistance) return;

            Spin(leftRoller);
            Spin(centerRoller, rollerSpinDelay);
            Spin(rightRoller, rollerSpinDelay * 2.0f);
        }
    }

    private void Spin(Transform roller, float spinDelay = 0.0f)
    {
        int symbol_index = Random.Range(0, symbols.Length);

        string target_symbol = symbols[symbol_index];

        Vector3 target_rotation = roller.transform.localRotation.eulerAngles;
        target_rotation.z = symbolRotations[symbol_index] + 360.0f;

        roller.transform.DOLocalRotate(target_rotation, rollerRotateDuration + spinDelay, RotateMode.FastBeyond360);
    }
}
