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
    public GameObject leftFrontBottom;
    public GameObject rightBackTop;
    public GameObject table;


    private Vector3 controllerPositionOld = new Vector3();
    public Vector3 controllerPositionChange = new Vector3();
    public bool isGripping = false;

    private float triggerValue;

    private void Start()
    {
        //levelParent = GameObject.FindGameObjectWithTag("LevelParent").GetComponent<NetworkObject>();
        //levelParent.RequestStateAuthority();
        leftFrontBottom = GameObject.Find("LeftFrontBottom");
        rightBackTop = GameObject.Find("RightBackTop");
        table = GameObject.FindGameObjectWithTag("Table");
    }

    void FixedUpdate()
    {
        if (!networkObject.HasInputAuthority) return;
        GetControllerInput();
        if (isGripping)
        {
            controllerPositionChange = controllerLeft.transform.position - controllerPositionOld;
            controllerPositionChange.y = 0f;
            //controllerPositionChange *= 0.5f;

            leftFrontBottom.transform.position -= controllerPositionChange;
            rightBackTop.transform.position -= controllerPositionChange;

            foreach(Material material in mapObjectMaterials)
            {
                material.SetVector("_LeftFrontBottom", leftFrontBottom.transform.position);
                material.SetVector("_RightBackTop", rightBackTop.transform.position);
            }
            
            transform.position -= controllerPositionChange;
            table.transform.position -= controllerPositionChange;
            controllerPositionOld = controllerLeft.transform.position;
        }
    }

    private void GetControllerInput()
    {
        //Check if left controller is trying to grip and move the world
        if (!controllerLeft.inputDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.trigger, out triggerValue)) return;
        if (triggerValue > 0.1f)
        {
            isGripping = true;
        } else 
        {
            isGripping = false;
            controllerPositionOld = controllerLeft.transform.position;
        }
    }
}
