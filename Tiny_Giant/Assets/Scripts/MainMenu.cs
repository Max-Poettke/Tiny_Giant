using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private GameObject volumeSettings;
    [SerializeField] private Button backButton;
    private bool _mouseClicked;
    private bool _optionsOpened;

    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
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
