using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.XR.CoreUtils;
public class ControllerCheckerVR : NetworkBehaviour
{
    public Camera curCamera;
    public AudioListener listener;
    public List<XRController> controllers = new List<XRController>();
    public XROrigin origin = null;
    
    void Start()
    {
        // Check wether the current NetworkPlayer has authority over this Object and if not then deactivate its input
        if (!Object.HasInputAuthority)
        {
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
