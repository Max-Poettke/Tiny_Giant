using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level2Trigger : NetworkBehaviour
{
    private CinemachineStoryboard _cine;
    [SerializeField] private float fadeTime;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _cine = other.GetComponentInChildren<CinemachineStoryboard>();
            if (_cine != null) StartCoroutine(LoadLoadingScreen());
        }
    }

    private IEnumerator LoadLoadingScreen()
    {
        var time = 0f;
        while (time < fadeTime)
        {
            _cine.m_Alpha = Mathf.Lerp(0f, 1f, time / fadeTime);
            time += Time.deltaTime;
            yield return null;
        }
        _cine.m_Alpha = 1f;
        Runner.LoadScene(SceneRef.FromIndex(3));
    }
    
    
}
