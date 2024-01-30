using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightableObject : MonoBehaviour
{
    private ParticleSystem _flame;
    private Light _light;
    [SerializeField] private BridgeRaiser bridge;
    public bool raining;
    public bool _lit = true;
    private bool _covered;
    private bool _changed;
    // Start is called before the first frame update
    void Start()
    {
        _flame = GetComponentInChildren<ParticleSystem>();
        _light = _flame.GetComponentInChildren<Light>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_lit || !other.CompareTag("Arrow")) return;
        
        if (other.GetComponent<Arrow>().lit)
        {
            Light();
        }
    }

    private void Update()
    {
        if (!_lit) return;
        Debug.DrawRay(transform.position + (Vector3.up * 2f), Vector3.up * 25f, Color.red);
        if (raining && !Physics.Raycast(transform.position + (Vector3.up * 2f), Vector3.up * 25f))
        {
            Extinguish();
        }
    }

    private void Light()
    {
        bridge.torchCount++;
        _lit = true;
        _flame.Play();
        _light.enabled = true;
    }

    private void Extinguish()
    {
        bridge.torchCount--;
        _lit = false;
        _flame.Stop();
        _light.enabled = false;
    }
}
