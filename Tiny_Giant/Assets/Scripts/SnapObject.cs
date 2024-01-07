using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

using System.Linq;

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
        try
        {
            obj = other.gameObject;
            objPlaceableObject = obj.GetComponent<PlaceableObject>();
            objRigidBody = obj.GetComponent<Rigidbody>();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
