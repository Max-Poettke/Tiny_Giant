using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class ButtonGateTrigger : NetworkBehaviour
{
    [SerializeField] private GameObject gate;

    [SerializeField] private float raiseTime;
    [SerializeField] private float lowerTime;
    [SerializeField] private float waitTime = 1f;

    [SerializeField] public bool grabbed;
    
    private enum GateState
    {
        Still, Raising, Falling
    }

    private GateState state;

    private Vector3 startPosition;
    
    void Start()
    {
        startPosition = gate.transform.position;
        state = GateState.Still;
    }

    private IEnumerator RaiseGate()
    {
        state = GateState.Raising;
        var time = 0f;
        Debug.Log("Raising Gate!");
        var endPosition = new Vector3(startPosition.x, startPosition.y + 8f, startPosition.z);
        while (time < raiseTime && state == GateState.Raising)
        {
            while (grabbed) { yield return null; }
            
            gate.transform.position = Vector3.Lerp(startPosition, endPosition, time / raiseTime);

            time += Time.deltaTime;

            yield return null;
        }
        yield return new WaitForSeconds(waitTime);
        StartCoroutine(LowerGate());
    }

    private IEnumerator LowerGate()
    {
        Debug.Log("Lowering Gate!");
        state = GateState.Falling;
        var time = 0f;

        var position = gate.transform.position;
        var endPosition = startPosition;
        while (time < lowerTime && state == GateState.Falling)
        {
            while (grabbed) { yield return null; }

            gate.transform.position = Vector3.Lerp(position, endPosition, time / lowerTime);

            time += Time.deltaTime;

            yield return null;
        }

        state = GateState.Still;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Arrow") && state != GateState.Falling)
        {
            StartCoroutine(RaiseGate());
        }
    }
}
