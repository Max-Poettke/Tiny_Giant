using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.XR.Interaction.Toolkit;
using Fusion.XR.Shared.Rig;

public class XRScrollMap : MonoBehaviour
{
    public HardwareHand hardwareHand;
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
        leftFrontBottom = GameObject.Find("LeftFrontBottom");
        rightBackTop = GameObject.Find("RightBackTop");
        table = GameObject.FindGameObjectWithTag("Table");
        if(!leftFrontBottom || !rightBackTop || !table)
        {
            Debug.LogError("XRScrollMap: Could not find LeftFrontBottom, RightBackTop, or Table GameObjects. Make sure they are in the scene and tagged correctly.");
            this.enabled = false;
        }
    }

    void FixedUpdate()
    {
        GetControllerInput();
        if (isGripping)
        {
            controllerPositionChange = hardwareHand.transform.position - controllerPositionOld;
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
            controllerPositionOld = hardwareHand.transform.position;
        }
    }

    private void GetControllerInput()
    {
        //Check if left controller is trying to grip and move the world
        triggerValue = hardwareHand.triggerAction.action.ReadValue<float>();
        if (triggerValue > 0.1f)
        {
            isGripping = true;
        } else 
        {
            isGripping = false;
            controllerPositionOld = hardwareHand.transform.position;
        }
    }
}
