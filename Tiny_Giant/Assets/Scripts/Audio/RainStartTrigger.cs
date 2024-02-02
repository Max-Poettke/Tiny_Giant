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
    private FirstPersonController _firstPersonController;
    private void OnTriggerEnter(Collider playerCollider)
    {
        if (playerCollider.tag.Equals("Player"))
        {
            playerCollider.GetComponent<RainIndicator>().isRaining = true;
            _firstPersonController = playerCollider.GetComponent<FirstPersonController>();
            _firstPersonController.deathPoint = new Vector3(-36f, 4f, -44f);
            StartCoroutine(RainEntrance());
        }
    }

    private IEnumerator RainEntrance()
    {
        _firstPersonController.ResetVelocity();
        _firstPersonController.playerCanMove = false;
        _firstPersonController.cameraCanMove = false;
        rain.Play();
        AudioManager.audioManagerInstance.StartRain();
        rainCam1.enabled = true;
        yield return new WaitForSecondsRealtime(5f);
        
        showBrazier.RPC_ToggleRaining();
        yield return new WaitForSecondsRealtime(.8f);
        
        rainCam2.enabled = true;
        yield return new WaitForSecondsRealtime(6.5f);
        rainCam1.enabled = false;
        rainCam2.enabled = false;
        _firstPersonController.playerCanMove = true;
        _firstPersonController.cameraCanMove = true;
        gameObject.SetActive(false);
    }
}
