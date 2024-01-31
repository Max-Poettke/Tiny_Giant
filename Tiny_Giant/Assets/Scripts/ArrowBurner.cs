using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBurner : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Arrow"))
        {
            other.GetComponent<Arrow>().RPC_Light();
        }
    }
}
