using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using Fusion;
using System.Net.NetworkInformation;

public class ButtonGateTrigger : NetworkBehaviour
{
    [SerializeField] private MoveUpAndDown _gate;
    [SerializeField] private CinemachineImpulseSource _impulseSource;
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Arrow"))
        {
            if(!_gate.inMotion) _impulseSource.GenerateImpulse();
            _gate.StartMove();
        }
    }
}
