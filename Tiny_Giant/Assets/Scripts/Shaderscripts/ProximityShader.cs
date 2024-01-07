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

    private void Start()
    {
        proximityMaterial = GetComponent<MeshRenderer>().materials[materialIndex];
    }
    
    private void Update()
    {
        Vector3 position = originalObjectTransform.position;
        proximityMaterial.SetVector("ObjectPos",position);
        proximityMaterial.SetFloat("MaxDist", maxDistance);
    }
}
