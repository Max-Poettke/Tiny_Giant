using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class MenuMusic : MonoBehaviour
{
    [field: SerializeField] public EventReference mainMenuMusic { get; private set; }
    private EventInstance mainMenuMusicInstance;

    private void Start()
    {
        mainMenuMusicInstance = RuntimeManager.CreateInstance(mainMenuMusic);
        mainMenuMusicInstance.start();
    }

    private void OnDestroy()
    {
        mainMenuMusicInstance.stop(STOP_MODE.IMMEDIATE);
        mainMenuMusicInstance.release();
    }
}
