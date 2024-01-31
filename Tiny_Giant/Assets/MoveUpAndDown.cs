using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using ExitGames.Client.Photon.StructWrapping;
using Fusion.XR.Shared.Rig;

public class MoveUpAndDown : NetworkBehaviour, IPlayerJoined
{
    public float distanceY;
    private Vector3 startPosition;
    private Vector3 endPosition;
    public float timeUp;
    public float timeDown;
    public float timeWait;
    [Networked]
    public bool isGrabbed { get; set; }

    private bool grabbed;
    public bool inMotion = false;

    private ChangeDetector changeDetector;
    
    private void Start() {
        startPosition = transform.position;
        endPosition = new Vector3(transform.position.x, transform.position.y + distanceY, transform.position.z);
    }

    public void StartMove(){
        if(!inMotion && !grabbed){
            StartCoroutine(MoveUp());
            inMotion = true;
        }
    }

    public override void Render()
    {
        base.Render();
        foreach (var change in changeDetector.DetectChanges(this))
    {
        switch (change)
        {
            case nameof(isGrabbed):
                grabbed = isGrabbed;
                break;
        }
    }
    }

    private IEnumerator MoveUp(){
        float timer = 0;
        while(!grabbed){
            timer += Runner.DeltaTime;
            Debug.Log(timer);
            transform.position = Vector3.Lerp(transform.position, endPosition, timer / timeUp);
            if(transform.position.y >= endPosition.y - 0.1f){
                break;
            }
            yield return new WaitForFixedUpdate();
        }
        yield return new WaitForSeconds(timeWait);
        StartCoroutine(MoveDown());
    }

    private IEnumerator MoveDown(){
        float timer = 0;
        while(!grabbed){
            timer += Runner.DeltaTime;
            transform.position = Vector3.Lerp(transform.position, startPosition, timer / timeDown);
            if(transform.position.y <= startPosition.y + 0.1f){
                break;
            }
            yield return new WaitForFixedUpdate();
        }
        inMotion = false;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Interactor")){
            var hand = other.transform.parent.GetComponent<HardwareHand>();
            hand.SendHapticImpulse(0.3f, 0.1f);
        }
    }

    private bool hasGrabbed = false;
    private void OnTriggerStay(Collider other) {
        if (other.gameObject.CompareTag("Interactor")){
            var hand = other.transform.parent.GetComponent<HardwareHand>();
            if(HasStateAuthority) isGrabbed = other.GetComponent<GrabbingState>().isGripping;
            if(isGrabbed && !hasGrabbed){
                hasGrabbed = true;
                hand.SendHapticImpulse(0.3f, 0.1f);
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Interactor")){
            if(HasStateAuthority) isGrabbed = false;
            hasGrabbed = false;
        }
    }

    void IPlayerJoined.PlayerJoined(PlayerRef player)
    {
        if(changeDetector.Equals(null))changeDetector = GetChangeDetector(ChangeDetector.Source.SimulationState);
    }
}
