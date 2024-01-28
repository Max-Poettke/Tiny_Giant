using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class SmallPlayerAudio : MonoBehaviour
{
    public static SmallPlayerAudio playerAudioInstance { get; private set; }
    private StudioEventEmitter playerFootstepsEmitter;

    public StudioEventEmitter getPlayerFootstepsEmitter()
    {
        return playerFootstepsEmitter;
    }

    private void Awake()
    {
        playerAudioInstance = this;
        
        playerFootstepsEmitter =
            AudioManager.audioManagerInstance.InitializeEventEmitter(FMODEvents.eventsInstance.playerFootsteps,
                this.gameObject);
    }

    public void UpdateSound(bool isWalking)
    {
        if (isWalking)
        {
            PLAYBACK_STATE playbackState;
            playerFootstepsEmitter.EventInstance.getPlaybackState(out playbackState);
            if (playbackState.Equals(PLAYBACK_STATE.STOPPED))
            {
                playerFootstepsEmitter.Play();
            }
        }
        else
        {
            playerFootstepsEmitter.Stop();
        }
    }
}
