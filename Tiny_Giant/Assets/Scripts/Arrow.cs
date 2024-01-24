using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody rb;
    private Material material;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        material = gameObject.GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(rb.velocity != Vector3.zero)
            rb.rotation = Quaternion.LookRotation(rb.velocity);
    }

    private void OnCollisionEnter(Collision other)
    {
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    public void Vanish()
    {
        StartCoroutine(VanishC());
    }

    public IEnumerator VanishC()
    {
        var color = material.color;
        for (float i = 1; i > 0f ; i -= 0.1f)
        {
            color.a = i;
            material.color = color;
            yield return new WaitForSeconds(0.1f);
        }
        Destroy(gameObject);
    }
}
