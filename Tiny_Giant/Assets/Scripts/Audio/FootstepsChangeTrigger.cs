using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepsChangeTrigger : MonoBehaviour
{
    [Header("Ground Type")]
    [SerializeField] private GroundType type;

    private void OnCollisionStay(Collision playerCollision)
    {
        if (playerCollision.transform.tag.Equals("Player"))
        {
            AudioManager.audioManagerInstance.SetGroundType(type);
        }
    }
}
