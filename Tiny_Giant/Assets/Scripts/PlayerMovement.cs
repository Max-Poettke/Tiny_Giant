using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class PlayerMovement : NetworkBehaviour
{
    private CharacterController _controller;
    public float Speed;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }
    
    public override void FixedUpdateNetwork()
    {
        if (HasStateAuthority == false) return;
        
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) *
                       Runner.DeltaTime * Speed;

        _controller.Move(move);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }
    }
}
