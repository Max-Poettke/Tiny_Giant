using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    private enum VolumeType
    {
        MASTER,
        MUSIC,
        AMBIENCE,
        SOUNDFX
    }

    [Header("Type")]
    [SerializeField]
    private VolumeType _volumeType;

    private Slider volumeSlider;

    private void Awake()
    {
        volumeSlider = GetComponent<Slider>();
        switch (_volumeType)
        {
            case VolumeType.MASTER:
                volumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", 1f);
                break;
            case VolumeType.MUSIC:
                volumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
                break;
            case VolumeType.AMBIENCE:
                volumeSlider.value = PlayerPrefs.GetFloat("AmbienceVolume", 1f);
                break;
            case VolumeType.SOUNDFX:
                volumeSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);
                break;
            default:
                Debug.LogWarning("Volume Type not supported: " + _volumeType);
                break;
        }
    }

    public void OnSliderValueChanged(float value)
    {
        switch (_volumeType)
        {
            case VolumeType.MASTER:
                AudioManager.audioManagerInstance.masterBus.setVolume(value);
                PlayerPrefs.SetFloat("MasterVolume", value);
                break;
            case VolumeType.MUSIC:
                AudioManager.audioManagerInstance.musicBus.setVolume(value);
                PlayerPrefs.SetFloat("MusicVolume", value);
                break;
            case VolumeType.AMBIENCE:
                AudioManager.audioManagerInstance.ambienceBus.setVolume(value);
                PlayerPrefs.SetFloat("AmbienceVolume", value);
                break;
            case VolumeType.SOUNDFX:
                AudioManager.audioManagerInstance.soundFXBus.setVolume(value);
                PlayerPrefs.SetFloat("SFXVolume", value);
                break;
            default:
                Debug.LogWarning("Volume Type not supported: " + _volumeType);
                break;
        }
    }
}
