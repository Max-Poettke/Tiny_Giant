using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{
    
    public static FMODEvents eventsInstance { get; private set; }

    private void Awake()
    {
        if (eventsInstance != null)
        {
            Debug.LogError("More than one FMOD Events instance in scene.");
        }
        eventsInstance = this;
    }
}
