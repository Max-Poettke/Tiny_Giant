using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(rb.velocity != Vector3.zero)
            rb.rotation = Quaternion.LookRotation(rb.velocity);
    }

    private void OnCollisionEnter(Collision other)
    {
        rb.isKinematic = true;
    }
}
