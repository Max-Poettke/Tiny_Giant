using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    private Input_Actions _inputActions;
    private InputAction _pauseMenu;

    [SerializeField] private GameObject pauseUI;
    [SerializeField] private bool isPaused;

    private void Awake()
    {
        _inputActions = new Input_Actions();
    }
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnEnable()
    {
        _pauseMenu = _inputActions.UISmallPlayer.Escape;
        _pauseMenu.Enable();
        _pauseMenu.performed += Pause;
    }

    private void OnDisable()
    {
        _pauseMenu.Disable();
    }

    void Pause(InputAction.CallbackContext context)
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            ActivatePauseMenu();
        }
        else
        {
            DeactivatePauseMenu();
        }
    }

    void ActivatePauseMenu()
    {
        Time.timeScale = 0;
        pauseUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }

    public void DeactivatePauseMenu()
    {
        Time.timeScale = 1;
        pauseUI.SetActive(false);
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
