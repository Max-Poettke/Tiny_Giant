// CHANGE LOG
// 
// CHANGES || version VERSION
//
// "Enable/Disable Headbob, Changed look rotations - should result in reduced camera jitters" || version 1.0.1

using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using FMOD;
using Fusion;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Unity.VisualScripting;
using Object = System.Object;
using FMOD.Studio;
using FMODUnity;
using Debug = UnityEngine.Debug;
using STOP_MODE = FMOD.Studio.STOP_MODE;

#if UNITY_EDITOR
    using UnityEditor;
    using System.Net;
#endif

public class FirstPersonController : NetworkBehaviour
{
    private Rigidbody rb;

    #region Camera Movement Variables

    public Camera playerCamera;
    [SerializeField] private CinemachineVirtualCamera cineCamera;

    public float fov = 60f;
    public bool invertCamera = false;
    public bool cameraCanMove = true;
    public float mouseSensitivity = 2f;
    public float maxLookAngle = 50f;

    // Crosshair
    public bool lockCursor = true;
    public bool crosshair = true;
    public Sprite crosshairImage;
    public Color crosshairColor = Color.white;

    // Internal Variables
    private float yaw = 0.0f;
    private float pitch = 0.0f;
    private Image crosshairObject;

    #region Camera Zoom Variables

    public bool enableZoom = true;
    public bool holdToZoom = false;
    public float zoomFOV = 35f;
    public float zoomStepTime = 3f;
    
    // Animation
    private Animator _animator;
    private NetworkMecanimAnimator _netAnimator;
    private static readonly int Walk = Animator.StringToHash("Walk");
    private static readonly int Run = Animator.StringToHash("Run");
    private static readonly int Jump = Animator.StringToHash("Jump");

    // Internal Variables
    private bool isZoomed = false;

    #endregion
    #endregion

    #region Movement Variables

    public bool playerCanMove = true;
    public float walkSpeed = 5f;
    public float maxVelocityChange = 10f;

    // Internal Variables
    private bool isWalking = false;

    #region Sprint

    public bool enableSprint = true;
    public bool unlimitedSprint = false;
    public float sprintSpeed = 7f;
    public float sprintDuration = 5f;
    public float sprintCooldown = .5f;
    public float sprintFOV = 70f;
    public float sprintFOVStepTime = 3f;

    // Sprint Bar
    public bool useSprintBar = true;
    public bool hideBarWhenFull = true;
    public Image sprintBarBG;
    public Image sprintBar;
    public float sprintBarWidthPercent = .3f;
    public float sprintBarHeightPercent = .015f;

    // Internal Variables
    private CanvasGroup sprintBarCG;
    private bool sprintPressed;
    private bool isSprinting = false;
    private float sprintRemaining;
    private float sprintBarWidth;
    private float sprintBarHeight;
    private bool isSprintCooldown = false;
    private float sprintCooldownReset;

    #endregion

    #region Jump

    public bool enableJump = true;
    public float jumpPower = 5f;

    // Internal Variables
    private bool isGrounded = false;

    #endregion

    #region Crouch

    public bool enableCrouch = true;
    public bool holdToCrouch = true;
    public float crouchHeight = .75f;
    public float speedReduction = .5f;

    // Internal Variables
    private bool isCrouched = false;
    private Vector3 originalScale;

    #endregion

    public Transform joint;
    #endregion


    #region InputSystem Shenanigans

    private Vector3 _moveVector;
    
    public void OnMove(InputAction.CallbackContext context)
    {
     //   Debug.Log("Trying to move");
        var input = context.ReadValue<Vector2>();
        _moveVector = new Vector3(input.x, 0f, input.y);
    }

    public void OnJump()
    {
        if (!enableJump || !isGrounded) return;
        
        // Adds force to the player rigidbody to jump
        if (isGrounded)
        {
            rb.AddForce(0f, jumpPower, 0f, ForceMode.Impulse);
            _netAnimator.SetTrigger(Jump, true);
            isGrounded = false;
        }

        // When crouched and using toggle system, will uncrouch for a jump
        if(isCrouched && !holdToCrouch)
        {
            Crouch();
        }
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (isSprinting) return;
        if (!enableCrouch) return;
        
        if(!holdToCrouch)
        {
            Crouch();
        }
            
        if(context.performed)
        {
            isCrouched = false;
            Crouch();
        }
        else if(context.canceled)
        {
            isCrouched = true;
            Crouch();
        }
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            sprintPressed = true;
        }
        else if (context.canceled)
        {
            sprintPressed = false;
            _animator.SetBool(Run, false);
        }
    }

    private Vector2 _mouse;
    public void OnLook(InputAction.CallbackContext context)
    {
        #region Camera

        // Control camera movement
        if(cameraCanMove)
        {
            _mouse = context.ReadValue<Vector2>();
            yaw = transform.localEulerAngles.y + _mouse.x * mouseSensitivity * 10f;
        }

        // Clamp pitch between lookAngle
        pitch = Mathf.Clamp(pitch, -maxLookAngle, maxLookAngle);
        
        #endregion
    }

    public void OnZoom(InputAction.CallbackContext context)
    {
        #region Camera Zoom

        if (!enableZoom) return;
        
        // Changes isZoomed when key is pressed
        // Behavior for toogle zoom
        if(!holdToZoom && !isSprinting)
        {
            if (!isZoomed)
            {
                isZoomed = true;
            }
            else
            {
                isZoomed = false;
            }
        }

        // Changes isZoomed when key is pressed
        // Behavior for hold to zoom
        if(holdToZoom && !isSprinting)
        {
            if(context.performed)
            {
                isZoomed = true;
            }
            else if(context.canceled)
            {
                isZoomed = false;
            }
        }

        #endregion
    }

    
    public BowChest _bowChest;
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!_bowChest) return;
        if (_bowChest._state is BowChest.ChestState.Opening or BowChest.ChestState.Empty) return;
        if(!HasInputAuthority) return;

        switch (_bowChest._state)
        {
            case BowChest.ChestState.Waiting:
                StartCoroutine(_bowChest.OpenChest());
                break;
            case BowChest.ChestState.Open:
                _bowChest.PickUpBow();
                break;
        }
    }
    
    
    #endregion

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _netAnimator = GetComponent<NetworkMecanimAnimator>();
        rb = GetComponent<Rigidbody>();
        crosshairObject = GetComponentInChildren<Image>();

        // Set internal variables
        playerCamera.fieldOfView = fov;
        originalScale = transform.localScale;

        if (!unlimitedSprint)
        {
            sprintRemaining = sprintDuration;
            sprintCooldownReset = sprintCooldown;
        }
    }

    public override void Spawned()
    {
        if(lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        if(crosshair)
        {
//            crosshairObject.sprite = crosshairImage;
//            crosshairObject.color = crosshairColor;
        }
        else
        {
            crosshairObject.gameObject.SetActive(false);
        }

        #region Sprint Bar

        sprintBarCG = GetComponentInChildren<CanvasGroup>();

        if(useSprintBar)
        {
            sprintBarBG.gameObject.SetActive(true);
//            sprintBar.gameObject.SetActive(true);

            float screenWidth = Screen.width;
            float screenHeight = Screen.height;

            sprintBarWidth = screenWidth * sprintBarWidthPercent;
            sprintBarHeight = screenHeight * sprintBarHeightPercent;

            sprintBarBG.rectTransform.sizeDelta = new Vector3(sprintBarWidth, sprintBarHeight, 0f);
//            sprintBar.rectTransform.sizeDelta = new Vector3(sprintBarWidth - 2, sprintBarHeight - 2, 0f);

            if(hideBarWhenFull)
            {
                sprintBarCG.alpha = 0;
            }
        }
        else
        {
            sprintBarBG.gameObject.SetActive(false);
            sprintBar.gameObject.SetActive(false);
        }

        #endregion
    }

    float camRotation;
    private float curFreq;
    private float curAmp;
    public override void FixedUpdateNetwork()
    {
        InputSystem.Update();
        if (!HasInputAuthority) return;
        transform.localEulerAngles = new Vector3(0, yaw, 0);
        #region Movement
        var noise = cineCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        if (playerCanMove)
        {
            // Calculate how fast we should be moving
            var targetVelocity = _moveVector;

            // Checks if player is walking and isGrounded
            // Will allow head bob
            if (targetVelocity.x != 0 || targetVelocity.z != 0 && isGrounded)
            {
                isWalking = true;
                _animator.SetBool(Walk, true);
                var mapVel = Mathf.Clamp(rb.velocity.magnitude, 0f, 5f);
                var norVel = Mathf.InverseLerp(0f, 5f, mapVel);
                var targetFrequency = Mathf.Lerp(.75f, 2f, norVel);
                curFreq = Mathf.Lerp(curFreq, targetFrequency, Time.deltaTime * 15f);

                
                var targetAmplitude = Mathf.Lerp(0.75f, 1f, norVel);
                curAmp = Mathf.Lerp(curAmp, targetAmplitude, Time.deltaTime * 15f);

                noise.m_FrequencyGain = curFreq;
                noise.m_AmplitudeGain = curAmp;
                
            }
            else
            {
                isWalking = false;
                _animator.SetBool(Walk, false);
                _animator.SetBool(Run, false);
                curFreq = Mathf.Lerp(curFreq, 0f, Time.deltaTime * 10f);
                curAmp = Mathf.Lerp(curAmp, 0f, Time.deltaTime * 10f);
                
                noise.m_FrequencyGain = curFreq;
                noise.m_AmplitudeGain = curAmp;
            }
            SmallPlayerAudio.playerAudioInstance.UpdateSound(isWalking);

            // All movement calculations while sprint is active
            if (enableSprint && sprintPressed && sprintRemaining > 0f && !isSprintCooldown)
            {
                targetVelocity = transform.TransformDirection(targetVelocity) * sprintSpeed;

                // Apply a force that attempts to reach our target velocity
                Vector3 velocity = rb.velocity;
                Vector3 velocityChange = (targetVelocity - velocity);
                velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
                velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
                velocityChange.y = 0;

                // Player is only moving when velocity change != 0
                // Makes sure fov change only happens during movement
                if (velocityChange.x != 0 || velocityChange.z != 0)
                {
                    if (_moveVector != Vector3.zero)
                    {
                        isSprinting = true;
                        _animator.SetBool(Run, true);
                    }
                    else
                    {
                        isSprinting = false;
                        _animator.SetBool(Run, false);
                    }

                    if (isCrouched)
                    {
                        Crouch();
                    }

                    if (hideBarWhenFull && !unlimitedSprint)
                    {
                        sprintBarCG.alpha += 5 * Runner.DeltaTime;
                    }
                }

                rb.AddForce(velocityChange, ForceMode.VelocityChange);
            }
            // All movement calculations while walking
            else
            {
                isSprinting = false;
                _animator.SetBool(Run, false);
                if (hideBarWhenFull && sprintRemaining == sprintDuration)
                {
//                    sprintBarCG.alpha -= 3 * Runner.DeltaTime;
                }

                targetVelocity = transform.TransformDirection(targetVelocity) * walkSpeed;

                // Apply a force that attempts to reach our target velocity
                Vector3 velocity = rb.velocity;
                Vector3 velocityChange = (targetVelocity - velocity);
                velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
                velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
                velocityChange.y = 0;

                rb.AddForce(velocityChange, ForceMode.VelocityChange);
            }
        }

        #endregion
    }

    public override void Render()
    {
        if (!HasInputAuthority) return;
        #region CameraZoom

        // Lerps camera.fieldOfView to allow for a smooth transistion
        if (isZoomed)
        {
            var curFOV = cineCamera.m_Lens.FieldOfView;
            curFOV = Mathf.Lerp(curFOV, zoomFOV, zoomStepTime * Time.deltaTime);
            cineCamera.m_Lens.FieldOfView = curFOV;
        }
        else if (!isZoomed && !isSprinting)
        {
            var curFOV = cineCamera.m_Lens.FieldOfView;
            curFOV = Mathf.Lerp(curFOV, fov, zoomStepTime * Time.deltaTime);
            cineCamera.m_Lens.FieldOfView = curFOV;
        }

        #endregion

        #region Sprint

        if (enableSprint)
        {
            if (isSprinting)
            {
                isZoomed = false;
                var curFOV = cineCamera.m_Lens.FieldOfView;
                curFOV = Mathf.Lerp(curFOV, sprintFOV, sprintFOVStepTime * Time.deltaTime);
                cineCamera.m_Lens.FieldOfView = curFOV;
                // Drain sprint remaining while sprinting
                if (!unlimitedSprint)
                {
                    sprintRemaining -= 1 * Runner.DeltaTime;
                    if (sprintRemaining <= 0)
                    {
                        isSprinting = false;
                        isSprintCooldown = true;
                    }
                }
            }
            else
            {
                // Regain sprint while not sprinting
                sprintRemaining = Mathf.Clamp(sprintRemaining += 1 * Runner.DeltaTime, 0, sprintDuration);
            }

            // Handles sprint cooldown 
            // When sprint remaining == 0 stops sprint ability until hitting cooldown
            if (isSprintCooldown)
            {
                sprintCooldown -= 1 * Runner.DeltaTime;
                if (sprintCooldown <= 0)
                {
                    isSprintCooldown = false;
                }
            }
            else
            {
                sprintCooldown = sprintCooldownReset;
            }

            // Handles sprintBar 
            if (useSprintBar && !unlimitedSprint)
            {
                float sprintRemainingPercent = sprintRemaining / sprintDuration;
                sprintBar.transform.localScale = new Vector3(sprintRemainingPercent, 1f, 1f);
            }
        }

        #endregion

        CheckGround();
    }

    private void LateUpdate()
    {
        if (!invertCamera)
        {
            pitch -= mouseSensitivity * _mouse.y;
            pitch = Mathf.Clamp(pitch, -maxLookAngle, maxLookAngle);
        }
        else
        {
            // Inverted Y
            pitch += mouseSensitivity * _mouse.y;
        }
        
        joint.localEulerAngles = new Vector3(pitch, 0, 0);
    }

    // Sets isGrounded based on a raycast sent straigth down from the player object
    private void CheckGround()
    {
        var position = transform.position;
        Vector3 origin = new Vector3(position.x, position.y + .1f, position.z);
        Vector3 direction = Vector3.down;
        float distance = .15f;

        if (Physics.Raycast(origin, direction, out RaycastHit hit, distance))
        {
            Debug.DrawRay(origin, direction * distance, Color.red);
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }
    private void Crouch()
    {
        // Stands player up to full height
        // Brings walkSpeed back up to original speed
        if(isCrouched)
        {
            transform.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z);
            walkSpeed /= speedReduction;

            isCrouched = false;
        }
        // Crouches player down to set height
        // Reduces walkSpeed
        else
        {
            transform.localScale = new Vector3(originalScale.x, crouchHeight, originalScale.z);
            walkSpeed *= speedReduction;

            isCrouched = true;
        }
    }
    
}