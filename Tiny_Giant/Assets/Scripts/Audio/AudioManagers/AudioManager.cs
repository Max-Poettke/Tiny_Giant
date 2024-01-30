using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using UnityEngine.Assertions.Must;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class AudioManager : MonoBehaviour
{
    public static AudioManager audioManagerInstance { get; private set; }
    private List<EventInstance> _eventInstances;
    private List<StudioEventEmitter> _eventEmitters;

    private EventInstance natureEventInstance;
    private EventInstance musicEventInstance;
    private EventInstance fireArrowMusicEventInstance;

    [Header("Volume")]
    [Range(0, 1)] 
    public float masterVolume = 1;
    [Range(0, 1)] 
    public float musicVolume = 1;
    [Range(0, 1)] 
    public float ambienceVolume = 1;
    [Range(0, 1)] 
    public float soundFXVolume = 1;

    private Bus masterBus;
    private Bus musicBus;
    private Bus ambienceBus;
    private Bus soundFXBus;

    private void Awake()
    {
        if (audioManagerInstance != null)
        {
            Debug.LogError("More than one Audio Manager in scene.");
        }

        audioManagerInstance = this;

        _eventInstances = new List<EventInstance>();
        _eventEmitters = new List<StudioEventEmitter>();

        masterBus = RuntimeManager.GetBus("bus:/");
        musicBus = RuntimeManager.GetBus("bus:/Music");
        ambienceBus = RuntimeManager.GetBus("bus:/Ambience");
        soundFXBus = RuntimeManager.GetBus("bus:/SoundFX");
    }

    private void Start()
    {
        InitializeNature(FMODEvents.eventsInstance.natureSounds);
        InitializeMusic(FMODEvents.eventsInstance.music);
    }

    private void Update()
    {
        masterBus.setVolume(masterVolume);
        musicBus.setVolume(musicVolume);
        ambienceBus.setVolume(ambienceVolume);
        soundFXBus.setVolume(soundFXVolume);
    }

    private void InitializeNature(EventReference natureEventReference)
    {
        natureEventInstance = CreateInstance(natureEventReference);
        if (natureEventReference.Path.EndsWith("Rain"))
        {
            return;
        }
        natureEventInstance.start();
    }

    public void StartRain()
    {
        natureEventInstance.start();
    }
    private void InitializeMusic(EventReference musicEventReference)
    {
        musicEventInstance = CreateInstance(musicEventReference);
        musicEventInstance.start();
    }

    #region ArrowOnFireMusic
    public void PlayFireArrowMusic(bool lit)
    {
        if (!lit || IsPlaying(fireArrowMusicEventInstance)) return;
        
        fireArrowMusicEventInstance = CreateInstance(FMODEvents.eventsInstance.fireArrowMusic);
        fireArrowMusicEventInstance.start();
    }

    public bool IsPlaying(FMOD.Studio.EventInstance instance)
    {
        instance.getPlaybackState(out var state);
        return state == PLAYBACK_STATE.PLAYING;
    }

    public void StopFireArrowMusic()
    {
        fireArrowMusicEventInstance.stop(STOP_MODE.ALLOWFADEOUT);
        fireArrowMusicEventInstance.release();
    }
    #endregion

    public void SetNatureArea(NatureArea area)
    {
        natureEventInstance.setParameterByName("NatureSoundType", (float)area);
    }

    public void SetGroundType(GroundType type)
    {
        SmallPlayerAudio.playerAudioInstance.getPlayerFootstepsEmitter().EventInstance.setParameterByName("GroundType",
            (float)type);
    }

    public EventInstance CreateInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        _eventInstances.Add(eventInstance);
        return eventInstance;
    }

    public StudioEventEmitter InitializeEventEmitter(EventReference eventReference, GameObject emitterGameObject)
    {
        StudioEventEmitter emitter = emitterGameObject.GetComponent<StudioEventEmitter>();
        emitter.EventReference = eventReference;
        _eventEmitters.Add(emitter);
        return emitter;
    }

    #region CleanUp
    private void CleanUp()
    {
        foreach (EventInstance eventInstance in _eventInstances)
        {
            eventInstance.stop(STOP_MODE.IMMEDIATE);
            eventInstance.release();
        }

        foreach (StudioEventEmitter emitter in _eventEmitters)
        {
            emitter.Stop();
        }
    }

    private void OnDestroy()
    {
        CleanUp();
    }
    #endregion
}
