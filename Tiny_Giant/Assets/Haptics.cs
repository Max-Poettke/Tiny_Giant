using System.Collections;
using System.Collections.Generic;
using Fusion.XR.Shared.Rig;
using UnityEngine;

public class Haptics : MonoBehaviour
{
    private HardwareHand hardwareHand;
    // Start is called before the first frame update
    void Start()
    {
        hardwareHand = transform.parent.GetComponent<HardwareHand>();
    }

    private void OnTriggerEnter(Collider other) {
        hardwareHand.SendHapticImpulse(0.3f, 0.5f);
    }
}
