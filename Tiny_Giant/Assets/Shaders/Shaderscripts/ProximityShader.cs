using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ProximityShader : MonoBehaviour
{
    public Transform originalObjectTransform;
    public float maxDistance;
    private Material proximityMaterial;
    public int materialIndex;
    private BoxCollider col;

    private void Start()
    {
        proximityMaterial = GetComponent<MeshRenderer>().materials[materialIndex];
        try
        {
            col = GetComponent<BoxCollider>();
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }
    
    private void Update()
    {
        if (col != null)
        {
            
        }
        
        Vector3 position = originalObjectTransform.position;
        proximityMaterial.SetVector("ObjectPos",position);
        proximityMaterial.SetFloat("MaxDist", maxDistance);
    }
}
