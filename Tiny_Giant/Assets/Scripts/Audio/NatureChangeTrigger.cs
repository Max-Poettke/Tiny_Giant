using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NatureChangeTrigger : MonoBehaviour
{
    [Header("Nature Area")] 
    [SerializeField] private NatureArea area;

    private void OnTriggerEnter(Collider playerCollider)
    {
        if (playerCollider.tag.Equals("Player"))
        {
            AudioManager.audioManagerInstance.SetNatureArea(area);
        }
    }
}
