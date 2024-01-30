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

    /// <summary>
    /// OnTriggerEnter is called when the Collider other enters the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Interactor"){
            StartTheGate();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Interactor"){
            StopTheGate();
        }
    }
}
