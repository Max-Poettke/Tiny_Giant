using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.XR.Interaction.Toolkit;

public class XRScrollMap : MonoBehaviour
{
    public XRController controllerLeft;
    public NetworkObject networkObject;
    public NetworkObject levelParent;
    public List<Material> mapObjectMaterials = new List<Material>();


    private Vector3 controllerPositionOld = new Vector3();
    public Vector3 controllerPositionChange = new Vector3();
    public bool isGripping = false;

    private float triggerValue;

    private void Start()
    {
        //levelParent = GameObject.FindGameObjectWithTag("LevelParent").GetComponent<NetworkObject>();
        levelParent.RequestStateAuthority();
    }

    void Update()
    {
        GetControllerInput();
        if (isGripping && networkObject.HasInputAuthority)
        {
            foreach(Material material in mapObjectMaterials)
            {
                Vector3 originalPosLeftFrontBottom = material.GetVector("_LeftFrontBottom");
                Vector3 originalPosRightBackTop = material.GetVector("_RightBackTop");
                Vector3 newPosLeftFrontBottom = UpdatePosition(originalPosLeftFrontBottom);
                Vector3 newPosRightBackTop = UpdatePosition(originalPosRightBackTop);
                material.SetVector("_LeftFrontBottom", newPosLeftFrontBottom);
                material.SetVector("_RightBackTop", newPosRightBackTop);
            } 
            controllerPositionOld = controllerLeft.transform.localPosition;
            transform.position -= controllerPositionChange;
        }
    }

    private void GetControllerInput()
    {
        //Check if left controller is trying to grip and move the world
        if (controllerLeft.inputDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.grip, out triggerValue) &&
            triggerValue > 0.1f)
        {
            isGripping = true;
            if (controllerPositionOld == Vector3.zero)
            {
                controllerPositionOld = controllerLeft.transform.localPosition;
            }
            controllerPositionChange = (controllerLeft.transform.localPosition - controllerPositionOld);
            controllerPositionChange.y = 0f;
            controllerPositionChange *= Quaternion.Angle(controllerLeft.transform.rotation, transform.rotation);
            controllerPositionChange *= 0.5f;

        }
        else
        {
            isGripping = false;
        }
    }
    
    private Vector3 UpdatePosition(Vector3 oldPos)
    {
        return oldPos + controllerPositionChange;
    }
}
