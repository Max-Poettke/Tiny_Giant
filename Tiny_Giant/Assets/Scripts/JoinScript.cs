using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinScript : MonoBehaviour
{
    public BasicSpawner spawner;

    private void OnTriggerEnter(Collider other)
    {
        spawner.JoinGameStart();
    }
}
