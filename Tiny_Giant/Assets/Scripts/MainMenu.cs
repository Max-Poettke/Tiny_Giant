using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using FMOD.Studio;
using FMODUnity;

public class MainMenu : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button backButton;

    [Header("Sliders")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider ambienceSlider;
    [SerializeField] private Slider sfxSlider;

    [SerializeField] private GameObject volumeSettings;
    private bool _mouseClicked;
    private bool _optionsOpened;
    
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


    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        masterBus = RuntimeManager.GetBus("bus:/");
        musicBus = RuntimeManager.GetBus("bus:/Music");
        ambienceBus = RuntimeManager.GetBus("bus:/Ambience");
        soundFXBus = RuntimeManager.GetBus("bus:/SoundFX");
        
        var prefMaster = PlayerPrefs.GetFloat("MasterVolume", 1f);
        var prefMusic = PlayerPrefs.GetFloat("MusicVolume", 1f);
        var prefAmbience = PlayerPrefs.GetFloat("AmbienceVolume", 1f);
        var prefSFX = PlayerPrefs.GetFloat("SFXVolume", 1f);
        
        masterBus.setVolume(prefMaster);
        masterSlider.value = prefMaster;
        
        musicBus.setVolume(prefMusic);
        musicSlider.value = prefMusic;
        
        ambienceBus.setVolume(prefAmbience);
        ambienceSlider.value = prefAmbience;
        
        soundFXBus.setVolume(prefSFX);
        sfxSlider.value = prefSFX;
    }

    public void OnMasterVolumeChanged(float value)
    {
        masterBus.setVolume(value);
        PlayerPrefs.SetFloat("MasterVolume", value);
    }
    
    public void OnMusicVolumeChanged(float value)
    {
        musicBus.setVolume(value);
        PlayerPrefs.SetFloat("MusicVolume", value);
    }
    
    public void OnAmbienceVolumeChanged(float value)
    {
        ambienceBus.setVolume(value);
        PlayerPrefs.SetFloat("AmbienceVolume", value);
    }
    
    public void OnSFXVolumeChanged(float value)
    {
        soundFXBus.setVolume(value);
        PlayerPrefs.SetFloat("SFXVolume", value);
    }

    public void OnNavigate()
    {
        if (_mouseClicked)
        {
            if (_optionsOpened)
            {
                backButton.Select();
            }
            else
            {
                startButton.Select(); 
            }
        }
        _mouseClicked = false;
    }

    public void OnClick()
    {
        _mouseClicked = true;
    }

    private void Update()
    {
        InputSystem.Update();
    }

    public void OnPlay()
    {
        SceneManager.LoadScene(1);
    }

    public void OnOptions()
    {
        startButton.gameObject.SetActive(false);
        optionsButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);
        volumeSettings.SetActive(true);
        if (!_mouseClicked) backButton.Select();
        _optionsOpened = true;
    }

    public void OnBack()
    {
        startButton.gameObject.SetActive(true);
        optionsButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);
        volumeSettings.SetActive(false);
        _optionsOpened = false;
        if (!_mouseClicked) optionsButton.Select();
    }

    public void OnQuit()
    {
        Application.Quit();
    }
}
