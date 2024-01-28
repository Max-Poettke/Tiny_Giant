using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Fusion;


public class Arrow : NetworkBehaviour
{
    // Start is called before the first frame update
    private Rigidbody rb;
    private Material material;

    //Networked variable for syncing the color change of the arrow
    [Networked]
    private Color color { get; set;}
    //Change detector for detecting changes in the color variable
    private ChangeDetector changeDetector;

    
    public override void Spawned()
    {
        //Get the change detector for the color variable
        changeDetector = GetChangeDetector(ChangeDetector.Source.SimulationState);
        _bow = transform.parent.parent.GetComponent<Bow>();
    }
    
    private GameObject _flame;
    public bool lit;
    private Bow _bow;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        material = gameObject.GetComponent<MeshRenderer>().material;
        _flame = GetComponentInChildren<FlameTag>(true).gameObject;
    }

    void Update()
    {
        //Detect changes in the color variable and apply them to the material
        foreach (var change in changeDetector.DetectChanges(this))
        {
            switch (change)
            {
                case nameof(color):
                    material.color = color;
                    break;
            }
        }
    }

    void FixedUpdate()
    {
        if(rb.velocity != Vector3.zero)
            rb.rotation = Quaternion.LookRotation(rb.velocity);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Arrow")) return;
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    public void Vanish()
    {
        StartCoroutine(VanishC());
    }

    private IEnumerator VanishC()
    {
        Debug.Log("Vanishing");
        color = material.color;
        for (float i = 1; i > 0f ; i -= 0.1f)
        {
            color = new Color(color.r, color.g, color.b, i);
            yield return new WaitForSeconds(0.1f);
        }
        Destroy(gameObject);
    }
    
    

    public void Light()
    {
        if (lit) return;
        lit = true;
        _flame.SetActive(true);
        _bow.fakeArrow.GetChild(0).gameObject.SetActive(true);
    }
    
    
}
