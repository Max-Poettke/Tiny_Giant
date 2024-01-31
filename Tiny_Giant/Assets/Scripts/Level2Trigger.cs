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
    [SerializeField] private bool endGame;
    private GameObject _player;
    private bool _loading;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _player = other.gameObject;
            _cine = other.GetComponentInChildren<CinemachineStoryboard>();
            if (_cine != null && !_loading)
            {
                _loading = true;
                StartCoroutine(endGame ? GoToMainMenu() : LoadLoadingScreen());
                _player.GetComponent<FirstPersonController>().playerCanMove = false;
            }
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
        Runner.LoadScene(SceneRef.FromIndex(3), LoadSceneMode.Additive);
       yield return new WaitForSeconds(1f);
        Runner.UnloadScene(SceneRef.FromIndex(2));
    }

    private IEnumerator GoToMainMenu()
    {
        var time = 0f;
        while (time < fadeTime)
        {
            _cine.m_Alpha = Mathf.Lerp(0f, 1f, time / fadeTime);
            time += Time.deltaTime;
            yield return null;
        }
        _cine.m_Alpha = 1f;
        Runner.Shutdown();
        Destroy(_player);
    }
    
    
}
