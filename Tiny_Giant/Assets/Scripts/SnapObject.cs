using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Fusion;

using System.Linq;
using Object = System.Object;

public class SnapObject : MonoBehaviour
{
    private XRDirectInteractor thisInteractor;
    public XRDirectInteractor otherInteractor;
    private GameObject obj;
    private PlaceableObject objPlaceableObject;
    private Rigidbody objRigidBody;

    public void Snap()
    {
        objRigidBody.isKinematic = false;
        if (otherInteractor.hasSelection) return;

        if (objPlaceableObject.canSnap)
        {
            obj.transform.position = objPlaceableObject.snapPos;
            obj.transform.rotation = objPlaceableObject.snapRotation;
            objRigidBody.isKinematic = true;
            
            foreach (var toPlaceObject in objPlaceableObject.targetObjects)
            {
                toPlaceObject.GetComponent<MeshRenderer>().enabled = false;
            }
        }
    }

    public void activatePlacesToPut()
    {
        foreach (var toPlaceObject in objPlaceableObject.targetObjects)
        {
            toPlaceObject.GetComponent<MeshRenderer>().enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("PlaceableObject")){
            obj = other.gameObject;
            objPlaceableObject = obj.GetComponentInChildren<PlaceableObject>();
            objRigidBody = obj.GetComponent<Rigidbody>();
            other.GetComponentInParent<NetworkObject>().RequestStateAuthority();
        }
    }
}
