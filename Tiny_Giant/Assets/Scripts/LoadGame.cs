using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGame : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(LoadLevel1());
    }

    private IEnumerator LoadLevel1()
    {
        var loadedScene = SceneManager.LoadSceneAsync("Level1");
        loadedScene.allowSceneActivation = false;
        while (!loadedScene.isDone)
        {
            if (loadedScene.progress >= 0.9f)
            {
                yield return new WaitForSecondsRealtime(4f);
                loadedScene.allowSceneActivation = true;
            }
            yield return null;
        }

    }
}
