using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class RequestPermission : NetworkBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlaceableObject"))
        {
            other.GetComponentInParent<NetworkObject>().RequestStateAuthority();
        }
    }
}
