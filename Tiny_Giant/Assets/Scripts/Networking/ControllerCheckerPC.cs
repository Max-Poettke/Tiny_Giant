using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.XR.CoreUtils;
public class ControllerCheckerPC : NetworkBehaviour
{
    public Camera curCamera;
    public AudioListener listener;
    public FirstPersonController firstPersonController;

    void Start()
    {
        // Check wether the current NetworkPlayer has authority over this Object and if not then deactivate its input
        if (!Object.HasInputAuthority)
        {
            //Deactivate PC Control
            if(!firstPersonController.Equals(null)) firstPersonController.enabled = false;
            curCamera.enabled = false;
            listener.enabled = false;
        }
    }
}
