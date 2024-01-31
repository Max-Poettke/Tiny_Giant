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
    private CinemachineImpulseSource _impulseSource;
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Arrow"))
        {
            _impulseSource.GenerateImpulse();
            _gate.StartMove();
        }
    }
}
