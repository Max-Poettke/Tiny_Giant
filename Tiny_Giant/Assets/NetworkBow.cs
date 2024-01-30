using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class NetworkBow : NetworkBehaviour
{
    private NewBow bowScript;
    public LocalCharacterMovement localCharacterMovement;

    public Transform fakeArrow;
    private Vector3 _fakeArrowPosition;

    [SerializeField] private GameObject fakeBow;
    
    public override void Spawned()
    {
        base.Spawned();
        localCharacterMovement = FindObjectOfType<LocalCharacterMovement>();
        bowScript = localCharacterMovement.transform.GetComponentInChildren<NewBow>();
        bowScript.networkBow = this;
        if (localCharacterMovement == null) Debug.LogError("Missing LocalCharacterMovement in the scene");
    
    }

    public void DeactivateFakeArrow(){
        fakeArrow.localPosition = _fakeArrowPosition;
        fakeArrow.gameObject.SetActive(false);
    }

    public void ActivateFakeArrow(){
        fakeArrow.gameObject.SetActive(true);
    }

    public void DeactivateFakeFlame(){
        fakeArrow.GetChild(0).gameObject.SetActive(false);
    }

    public void OnEnable(){
        fakeBow.SetActive(true);
    }

    public NetworkObject SpawnArrow(NetworkObject arrows){
        var arrow = Runner.Spawn(arrows, bowScript.transform.position + transform.TransformVector(0.03f, 0f, 0.3f), transform.rotation, Runner.LocalPlayer,
               (runner, no) => no.transform.parent = bowScript.transform);
        arrow.transform.SetParent(null);
        arrow.GetComponent<Rigidbody>().isKinematic = false;
        arrow.GetComponent<Rigidbody>().useGravity = true;
        arrow.GetComponent<Rigidbody>().AddForce(arrow.transform.forward * 50f, ForceMode.Impulse);
        return arrow;
    }

    public void DestroyArrow(NetworkObject arrow){
        Runner.Despawn(arrow);
    }
}
