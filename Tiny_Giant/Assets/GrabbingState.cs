using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion.XR.Shared.Rig;

public class GrabbingState : MonoBehaviour
{
    public bool isGripping = false;
    private float triggerValue;
    public HardwareHand hardwareHand;

    private void Update() {
        triggerValue = hardwareHand.triggerAction.action.ReadValue<float>();
        if (triggerValue > 0.1f)
        {
            isGripping = true;
        } else 
        {
            isGripping = false;
        }    
    }
}
