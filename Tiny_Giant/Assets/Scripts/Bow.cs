using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Fusion;
using Fusion.Addons.Physics;

public class Bow : NetworkBehaviour
{
    public GameObject arrows;
    public float maxHoldDuration = 2.5f;
    public float minHoldDuration = 0.2f;
    private NetworkObject arrow;
    public Transform cam;
    
    private float start = 0f;
    private float end = 0f;

    [SerializeField] private Vector3 initialPos;
    [SerializeField] private Vector3 raisedPos;
    [SerializeField] private float raiseTime;
    private bool _ready;

    [SerializeField] private float shotPower = 2f;

    [SerializeField] private Transform arrowParent;

    public Transform fakeArrow;
    private Vector3 _fakeArrowPosition;

    private Animator _animator;
    private Animator _playerAnimator;
    
    private GameObject[] activeArrows;
    [SerializeField] private int maxActiveArrows = 10;
    private int pointer;
    
    private Coroutine drawBow;
    private static readonly int Draw = Animator.StringToHash("Draw");
    private static readonly int Cancel = Animator.StringToHash("Cancel");
    private static readonly int Release = Animator.StringToHash("Release");

    private bool _cancelling;
    
    private static readonly int AimBow = Animator.StringToHash("AimBow");

    [SerializeField] private GameObject fakeBow;

    private static readonly int ShotBow = Animator.StringToHash("ShotBow");

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _playerAnimator = transform.root.GetComponent<Animator>();
        activeArrows = new GameObject[maxActiveArrows];
        _fakeArrowPosition = fakeArrow.localPosition;
    }

    private void OnEnable()
    {
        _ready = false;
        RPC_EnableFakeBow();
        StartCoroutine(RaiseBow());
    }
    
    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_EnableFakeBow()
    {
        fakeBow.SetActive(true);
    }

    private bool _firstShot;
    public void OnShoot(InputAction.CallbackContext context)
    {
        if (!gameObject.activeSelf || !_ready) return;
        
        if (context.performed)
        {
            _firstShot = true;
            var fpcontroller = transform.root.GetComponent<FirstPersonController>();
            fpcontroller.enableJump = false;
            fpcontroller.enableSprint = false;
            _playerAnimator.SetBool(ShotBow, false); 
            start = Time.time;
           // arrow = Instantiate(arrows, transform.position + transform.TransformVector(0f, 0f, 0.3f), transform.rotation, transform.parent);
           arrow = Runner.Spawn(arrows, arrowParent.transform.position + transform.TransformVector(0.03f, 0f, 0.3f), transform.rotation, Runner.LocalPlayer,
               (runner, no) => no.transform.parent = arrowParent);
           
           if(activeArrows[pointer] != null) {
                activeArrows[pointer].GetComponent<Arrow>().RPC_Vanish();
            } 
           
           activeArrows[pointer++] = arrow.gameObject; 
           pointer %= maxActiveArrows;
           
           drawBow = StartCoroutine(DrawBow());
           SmallPlayerAudio.playerAudioInstance.StretchBow();
        }

        
        if (context.canceled)
        {
            if (!_firstShot) return;
            var fpcontroller = transform.root.GetComponent<FirstPersonController>();
            fpcontroller.enableJump = true;
            fpcontroller.enableSprint = true;
            if (_cancelling) return;
            _cancelling = true;
            end = Time.time;
            _playerAnimator.SetBool(AimBow, false);
            AudioManager.audioManagerInstance.StopFireArrowMusic();
            if (end - start < minHoldDuration)
            {
                /*arrow.TryGetComponent(out Arrow a);
                if(a) a.Vanish();*/
                Runner.Despawn(arrow);
                _animator.SetTrigger(Cancel);
                _animator.ResetTrigger(Draw);
                _animator.ResetTrigger(Release);
                StopCoroutine(drawBow);
            }
            else
            {
                if(activeArrows[pointer] != null) activeArrows[pointer].GetComponent<Arrow>().RPC_Vanish();
                activeArrows[pointer++] = arrow.gameObject;
                pointer %= maxActiveArrows; 
                
                StopCoroutine(drawBow);
                _animator.ResetTrigger(Draw);
                _animator.ResetTrigger(Cancel);
                _animator.SetTrigger(Release);
                _playerAnimator.SetBool(ShotBow, true);
                var rb = arrow.GetComponent<Rigidbody>();
                arrow.GetComponent<Arrow>().RPC_ToggleTrail();
                AudioManager.audioManagerInstance.StopFireArrowMusic();
                rb.isKinematic = false;
                rb.useGravity = true;
                var holdMultiplier = Mathf.Min(end - start, maxHoldDuration);
                rb.AddForce((cam.forward) * shotPower * holdMultiplier, ForceMode.Impulse);
                arrow.transform.SetParent(null);

                SmallPlayerAudio.playerAudioInstance.ReleaseBow();
            }

            fakeArrow.localPosition = _fakeArrowPosition;
            fakeArrow.gameObject.SetActive(false);
            _cancelling = false;
        }
    }


    private IEnumerator DrawBow()
    {
        fakeArrow.GetChild(0).gameObject.SetActive(false);
        _animator.ResetTrigger(Cancel);
        _animator.ResetTrigger(Release);
        _animator.SetTrigger(Draw);
        var start1 = Time.time;
        _playerAnimator.SetBool(AimBow, true);
        fakeArrow.gameObject.SetActive(true);
        
        while(Time.time - start1 < 1f)
        {
            arrow.transform.Translate(new Vector3(0, 0, -0.18f) * (Time.deltaTime));
            fakeArrow.Translate(new Vector3(0, 0, -0.18f) * (Time.deltaTime));
            yield return null;
        }
        
        while(Time.time - start1 < 2.5f)
        {
            arrow.transform.Translate(new Vector3(0, 0, -0.18f) * (Time.deltaTime));
            fakeArrow.Translate(new Vector3(0, 0, -0.18f) * (Time.deltaTime));
            yield return null;
        }
    }

    private IEnumerator RaiseBow()
    {
        var time = 0f;
        while (time < raiseTime)
        {
            transform.localPosition = Vector3.Slerp(initialPos, raisedPos, time / raiseTime);
            time += Time.deltaTime;
            yield return null;
        }

        _ready = true;
    }
}
