using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopGate : MonoBehaviour
{
    public ButtonGateTrigger gate;

    public void StopTheGate(){
        gate.grabbed = true;
    }

    public void StartTheGate(){
        gate.grabbed = false;
    }
}
