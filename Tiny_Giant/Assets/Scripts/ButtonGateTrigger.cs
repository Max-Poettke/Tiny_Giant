using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using Fusion;

public class ButtonGateTrigger : NetworkBehaviour
{
    [SerializeField] private GameObject gate;

    [SerializeField] private float raiseTime;
    [SerializeField] private float lowerTime;
    [SerializeField] private float waitTime = 1f;

    [Networked]
    [SerializeField] public bool grabbed { get; set; } = false;
    private CinemachineImpulseSource _impulseSource;
    private enum GateState
    {
        Still, Raising, Falling
    }

    [Networked]
    private GateState state{ get; set;}

    private Vector3 startPosition;
    
    void Start()
    {
        startPosition = gate.transform.position;
        state = GateState.Still;
        _impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private IEnumerator RaiseGate()
    {
        state = GateState.Raising;
        var time = 0f;
        var endPosition = new Vector3(startPosition.x, startPosition.y + 10f, startPosition.z);
        while (time < raiseTime && state == GateState.Raising)
        {
            if(grabbed) continue;
            
            gate.transform.position = Vector3.Lerp(startPosition, endPosition, time / raiseTime);

            time += Time.deltaTime;

            yield return null;
        }

        gate.transform.position = endPosition;
        yield return new WaitForSeconds(waitTime);
        StartCoroutine(LowerGate());
    }

    private IEnumerator LowerGate()
    {
        state = GateState.Falling;
        var time = 0f;

        var position = gate.transform.position;
        var endPosition = startPosition;
        while (time < lowerTime && state == GateState.Falling)
        {
            if(grabbed) continue;

            gate.transform.position = Vector3.Lerp(position, endPosition, time / lowerTime);

            time += Time.deltaTime;

            yield return null;
        }

        gate.transform.position = startPosition;
        state = GateState.Still;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Arrow") && state != GateState.Falling)
        {
            _impulseSource.GenerateImpulse();
            StartCoroutine(RaiseGate());
        }
    }


    private void Update()
    {
        TestFunction();
    }
    private void TestFunction(){
        //constantly move the gate up and down
        if(state == GateState.Still){
            StartCoroutine(RaiseGate());
        }
    }
}
