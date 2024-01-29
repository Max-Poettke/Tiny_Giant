using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainStartTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider playerCollider)
    {
        if (playerCollider.tag.Equals("Player"))
        {
            AudioManager.audioManagerInstance.StartRain();
            this.gameObject.SetActive(false);
        }
    }
}