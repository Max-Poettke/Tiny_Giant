using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Fusion;
using UnityEngine.UI;
using Cinemachine;
using Debug = UnityEngine.Debug;
public enum PlayerPart{
    Body,
    Camera,
    Bow
}

public struct PlayerState{
    public Vector3 bodyPosition;
    public Quaternion bodyRotation;
    public Vector3 lookDirection;
    public bool isShooting;
}

public class LocalCharacterMovement : MonoBehaviour
{

    public CinemachineVirtualCamera _camera;
    public Transform joint;
    PlayerState playerState = default;

    public virtual PlayerState PlayerState{
        get{
            playerState.bodyPosition = transform.position;
            playerState.bodyRotation = transform.rotation;
            playerState.isShooting = isShooting;
            playerState.lookDirection = _camera.transform.forward;
            return playerState;
        }
    }
    
    
    public virtual void Rotate(float angle){
        transform.RotateAround(_camera.transform.position, transform.up, angle);
    }

    Rigidbody _rigidbody;
    private Vector3 _moveVector;
    private float movementSpeed = 5f;
    public void OnMove(InputAction.CallbackContext context)
    {
     //   Debug.Log("Trying to move");
        var input = context.ReadValue<Vector2>();
        _moveVector = new Vector3(input.x, 0f, input.y);
    }

    private bool isJumping;
    public float jumpForce = 5f;
    private bool isGrounded;
    public void OnJump(InputAction.CallbackContext context)
    {
        if (!isGrounded) return;
        if (context.started)
        {
            isJumping = true;
            _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, jumpForce, _rigidbody.velocity.z);
        } else if(context.canceled){
            isJumping = false;
            //_rigidbody.velocity = new Vector3(_rigidbody.velocity.x, -1f, _rigidbody.velocity.z);
        }
    }
    private bool isShooting;
    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isShooting = true;
        }
        else if (context.canceled)
        {
            isShooting = false;
        }
    }

    public float sprintSpeed = 10f;
    public float normalSpeed = 5f;
    private bool isSprinting = false;

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isSprinting = true;
        }
        else if (context.canceled)
        {
            isSprinting = false;
        }
    }

    private float pitch;
    private float yaw;
    private float rotationSpeed = 0.06f;
    public void OnLook(InputAction.CallbackContext context)
    {
        var input = context.ReadValue<Vector2>();
        pitch = input.y * rotationSpeed;
        yaw = input.x * rotationSpeed;
    }

    public void OnZoom(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _camera.m_Lens.FieldOfView = 60f;
        }
        else if (context.canceled)
        {
            _camera.m_Lens.FieldOfView = 80f;
        }
    }

    public BowChest _bowChest;
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!_bowChest) return;
        if (_bowChest._state is BowChest.ChestState.Opening or BowChest.ChestState.Empty) return;

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

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        float speed = isSprinting ? sprintSpeed : normalSpeed;
        // Use the speed value in your movement logic
        // For example, you can multiply it with the move vector
        Vector3 moveDirection = _moveVector.normalized * speed * Time.deltaTime;
        transform.Translate(moveDirection);


        // Clamp the pitch value to limit the camera rotation
        pitch = Mathf.Clamp(pitch, -90f, 90f);
        MoveCamera();
    }

    private void FixedUpdate()
    {
        InputSystem.Update();
        MoveCharacter();
    }

    private void MoveCharacter()
    {
        // Normalize the move vector to ensure consistent movement speed in all directions
        _moveVector.Normalize();
        _rigidbody.AddForce(_moveVector * movementSpeed * Time.fixedDeltaTime, ForceMode.VelocityChange);
        //_rigidbody.AddForce(Vector3.down * 9.81f * Time.fixedDeltaTime, ForceMode.VelocityChange);
        CheckGround();
    }
    private void MoveCamera()
    {   
        // Rotate the character transform along the y axis
        transform.Rotate(0f, yaw, 0f);
        // Rotate the camera transform along the x axis
        joint.Rotate(-pitch, 0f, 0f);
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
}
