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
        volumeSlider = this.GetComponent<Slider>();
    }

    private void Update()
    {
        switch (_volumeType)
        {
            case VolumeType.MASTER:
                volumeSlider.value = AudioManager.audioManagerInstance.masterVolume;
                break;
            case VolumeType.MUSIC:
                volumeSlider.value = AudioManager.audioManagerInstance.musicVolume;
                break;
            case VolumeType.AMBIENCE:
                volumeSlider.value = AudioManager.audioManagerInstance.ambienceVolume;
                break;
            case VolumeType.SOUNDFX:
                volumeSlider.value = AudioManager.audioManagerInstance.soundFXVolume;
                break;
            default:
                Debug.LogWarning("Volume Type not supported: " + _volumeType);
                break;
        }
    }

    public void OnSliderValueChanged()
    {
        switch (_volumeType)
        {
            case VolumeType.MASTER:
                AudioManager.audioManagerInstance.masterVolume = volumeSlider.value;
                break;
            case VolumeType.MUSIC:
                AudioManager.audioManagerInstance.musicVolume = volumeSlider.value;
                break;
            case VolumeType.AMBIENCE:
                AudioManager.audioManagerInstance.ambienceVolume = volumeSlider.value;
                break;
            case VolumeType.SOUNDFX:
                AudioManager.audioManagerInstance.soundFXVolume = volumeSlider.value;
                break;
            default:
                Debug.LogWarning("Volume Type not supported: " + _volumeType);
                break;
        }
    }
}
