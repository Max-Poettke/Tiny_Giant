using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class RainStartTrigger : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera rainCam1;
    [SerializeField] private CinemachineVirtualCamera rainCam2;
    [SerializeField] private LightableObject showBrazier;
    [SerializeField] private ParticleSystem rain;
    private void OnTriggerEnter(Collider playerCollider)
    {
        if (playerCollider.tag.Equals("Player"))
        {
            playerCollider.GetComponent<RainIndicator>().isRaining = true;
            StartCoroutine(RainEntrance());
        }
    }

    private IEnumerator RainEntrance()
    {
        rain.Play();
        AudioManager.audioManagerInstance.StartRain();
        rainCam1.enabled = true;
        yield return new WaitForSecondsRealtime(5f);
        
        showBrazier.raining = true;
        yield return new WaitForSecondsRealtime(.8f);
        
        rainCam2.enabled = true;
        yield return new WaitForSecondsRealtime(6.5f);
        rainCam1.enabled = false;
        rainCam2.enabled = false;
        gameObject.SetActive(false);
    }
}
