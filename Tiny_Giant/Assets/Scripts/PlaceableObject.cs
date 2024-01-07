using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PlaceableObject : MonoBehaviour
{
    public List<GameObject> targetObjects = new List<GameObject>();
    public bool canSnap = true;
    public Vector3 snapPos = new Vector3();
    public Quaternion snapRotation = new Quaternion();
    public XRGrabInteractable grabInteractable;

    private void OnTriggerEnter(Collider other)
    {
        foreach (GameObject g in targetObjects)
        {
            if (other.gameObject.Equals(g))
            {
                var otherTransform = other.transform;
                snapPos = otherTransform.position;
                snapRotation = otherTransform.rotation;
                canSnap = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        canSnap = false;
    }
}
