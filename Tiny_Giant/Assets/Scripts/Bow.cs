using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class Bow : MonoBehaviour
{
    public GameObject arrows;
    private GameObject arrow;
    [SerializeField] private float minHoldDuration = 0.5f;
    public float maxHoldDuration = 2.5f;
    
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
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _playerAnimator = transform.parent.parent.GetComponent<Animator>();
        activeArrows = new GameObject[maxActiveArrows];
        _fakeArrowPosition = fakeArrow.localPosition;
    }

    private void OnEnable()
    {
        _ready = false;
        fakeBow.SetActive(true);
        StartCoroutine(RaiseBow());
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (!gameObject.activeSelf || !_ready) return;
        
        if (context.performed)
        {
            start = Time.time;
            arrow = Instantiate(arrows, transform.position + transform.TransformVector(0f, 0f, 0.3f), transform.rotation, arrowParent);
            drawBow = StartCoroutine(DrawBow());
        }

        
        if (context.canceled)
        {
            if (_cancelling) return;
            _cancelling = true;
            end = Time.time;
            _playerAnimator.SetBool(AimBow, false);
            if (end - start < minHoldDuration)
            {
                Destroy(arrow);
                _animator.SetTrigger(Cancel);
                _animator.ResetTrigger(Draw);
                _animator.ResetTrigger(Release);
                StopCoroutine(drawBow);
                // animation.Stop();
            }
            else
            {
                if(activeArrows[pointer] != null) activeArrows[pointer].GetComponent<Arrow>().Vanish();
                activeArrows[pointer++] = arrow;
                pointer %= maxActiveArrows; 
                
                StopCoroutine(drawBow);
                _animator.ResetTrigger(Draw);
                _animator.ResetTrigger(Cancel);
                _animator.SetTrigger(Release);
                var rb = arrow.GetComponent<Rigidbody>();
                rb.isKinematic = false;
                rb.useGravity = true;
                var holdMultiplier = Mathf.Min(end - start, maxHoldDuration);
                rb.AddForce((cam.forward - cam.right / 35) * shotPower * holdMultiplier, ForceMode.Impulse); 
                arrow.transform.SetParent(null);
                
                // animation.Play("Release", PlayMode.StopAll);
                // animation["Release"].speed = 5f;
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
        yield return new WaitForSeconds(minHoldDuration);
        var start1 = Time.time;
        _playerAnimator.SetBool(AimBow, true);
        fakeArrow.gameObject.SetActive(true);
        /*animation.Play("DrawBow", PlayMode.StopAll);
        animation["DrawBow"].speed = 0.4f;*/
        while(Time.time - start1 < 1f)
        {
            arrow.transform.Translate(new Vector3(0, 0, -0.18f) * Time.deltaTime);
            fakeArrow.Translate(new Vector3(0, 0, -0.18f) * Time.deltaTime);
            yield return null;
        }
        // animation["DrawBow"].speed = 0.2f;
        while(Time.time - start1 < 2.5f)
        {
            arrow.transform.Translate(new Vector3(0, 0, -0.18f) * Time.deltaTime);
            fakeArrow.Translate(new Vector3(0, 0, -0.18f) * Time.deltaTime);
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
