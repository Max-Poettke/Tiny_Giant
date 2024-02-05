using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class LightableObject : NetworkBehaviour
{
    private ParticleSystem _flame;
    private Light _light;
    [SerializeField] private BridgeRaiser bridge;
    [SerializeField] private FMODUnity.StudioEventEmitter lightChord;
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
            RPC_Light();
        }
    }

    private void Update()
    {
        if (!_lit) return;
        Debug.DrawRay(transform.position + (Vector3.up * 2f), Vector3.up * 25f, Color.red);
        if (raining && !Physics.Raycast(transform.position + (Vector3.up * 2f), Vector3.up * 25f))
        {
            RPC_Extinguish();
        }
    }

    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_Light()
    {
        bridge.torchCount++;
        _lit = true;
        _flame.Play();
        lightChord.Play();
        _light.enabled = true;
    }

    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_ToggleRaining()
    {
        raining = true;
    }

    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    private void RPC_Extinguish()
    {
        bridge.torchCount--;
        _lit = false;
        _flame.Stop();
        _light.enabled = false;
    }
}
