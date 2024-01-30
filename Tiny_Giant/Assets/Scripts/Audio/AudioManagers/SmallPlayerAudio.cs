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
    private StudioEventEmitter bowSoundsEmitter;

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

        bowSoundsEmitter = AudioManager.audioManagerInstance.InitializeEventEmitter(FMODEvents.eventsInstance.bowSounds,
            this.gameObject.transform.Find("Joint/Rigged_Bow_NativeAmerican_Testing").gameObject);
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

    public void StretchBow()
    {
        bowSoundsEmitter.Play();
    }

    public void ReleaseBow()
    {
        bowSoundsEmitter.Play();
        bowSoundsEmitter.SetParameter("Bow",1);
    }
}
