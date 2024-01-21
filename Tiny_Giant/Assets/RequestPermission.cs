using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class RequestPermission : NetworkBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        try
        {
            other.GetComponentInParent<NetworkObject>().RequestStateAuthority();
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
        }
    }
}
