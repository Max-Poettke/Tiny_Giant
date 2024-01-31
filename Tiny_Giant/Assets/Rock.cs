using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class Rock : NetworkBehaviour
{
    public GameObject explosion;
    private void Update() {
        if (transform.position.y < -10) {
            Runner.Despawn(gameObject.GetComponent<NetworkObject>());
        }
    }
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Interactor")) {
            Runner.Despawn(gameObject.GetComponent<NetworkObject>());
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
    }
}
