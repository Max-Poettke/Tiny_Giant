using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.XR.CoreUtils;
public class ControllerChecker : NetworkBehaviour
{
    public Camera curCamera;
    public AudioListener listener;
    public FirstPersonController firstPersonController;
    public List<XRController> controllers = new List<XRController>();
    public XROrigin origin;
    
    /*
    private void Awake()
    {
        // Check wether the current NetworkPlayer has authority over this Object and if not then deactivate its input
        if (!Object.HasInputAuthority)
        {
            //Deactivate PC Control
            if(!firstPersonController.Equals(null)) firstPersonController.enabled = false;
            //Deactivate VR Control
            foreach (XRController controller in controllers)
            {
                controller.enabled = false;
            }
            if(!origin.Equals(null)) origin.enabled = false;
            
            curCamera.enabled = false;
            listener.enabled = false;
        }
        else
        {
            if(!firstPersonController.Equals(null)) firstPersonController.enabled = true;
            
            foreach (XRController controller in controllers)
            {
                controller.enabled = true;
            }
            if(!origin.Equals(null)) origin.enabled = true;
            
            curCamera.enabled = true;
            listener.enabled = true;
        }
    }
    */

    void Start()
    {
        // Check wether the current NetworkPlayer has authority over this Object and if not then deactivate its input
        if (!Object.HasInputAuthority)
        {
            Debug.Log("Deactivated control");
            //Deactivate PC Control
            if(!firstPersonController.Equals(null)) firstPersonController.enabled = false;
            //Deactivate VR Control
            foreach (XRController controller in controllers)
            {
                controller.enabled = false;
            }
            if(!origin.Equals(null)) origin.enabled = false;
            curCamera.enabled = false;
            listener.enabled = false;
        }
    }
}
