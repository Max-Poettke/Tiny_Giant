using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightableObject : MonoBehaviour
{
    private ParticleSystem _flame;
    [SerializeField] private BridgeRaiser bridge;
    private bool _lit;
    // Start is called before the first frame update
    void Start()
    {
        _flame = GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_lit || !other.CompareTag("Arrow")) return;
        
        if (other.GetComponent<Arrow>().lit)
        {
            Light();
        }
    }

    private void Light()
    {
        bridge.torchCount++;
        _lit = true;
        _flame.Play();
    }
}
