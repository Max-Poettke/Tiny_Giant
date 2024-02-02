using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using Fusion;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : NetworkBehaviour
{
    private Input_Actions _inputActions;
    private InputAction _pauseMenu;
    private PlayerInput _playerInput;

    [SerializeField] private GameObject pauseUI;
    private SlidePauseMenu _pauseSlide;
    private bool isPaused;

    private void Awake()
    {
        _inputActions = new Input_Actions();
    }
    private void Start()
    {
        _pauseSlide = pauseUI.GetComponent<SlidePauseMenu>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnEnable()
    {
        _pauseMenu = _inputActions.UI.Escape;
        _pauseMenu.Enable();
        _pauseMenu.performed += Pause;
    }

    private void OnDisable()
    {
        _pauseMenu.Disable();
    }

    private void Pause(InputAction.CallbackContext context)
    {
        if (!_playerInput) _playerInput = GameObject.FindWithTag("Player").GetComponent<PlayerInput>();
        if (_pauseSlide.moving) return;
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

    public void Pause()
    {
        if (!_playerInput) _playerInput = GameObject.FindWithTag("Player").GetComponent<PlayerInput>();
        if (_pauseSlide.moving) return;
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

    public void QuitToMainMenu()
    {
        Runner.Shutdown();
    }

    private void ActivatePauseMenu()
    {
        pauseUI.SetActive(true);
        Cursor.visible = true;
        _playerInput.actions.Disable();
        _playerInput.SwitchCurrentActionMap("UI");
        Cursor.lockState = CursorLockMode.None;
    }

    private void DeactivatePauseMenu()
    {
        StartCoroutine(_pauseSlide.SlideDown());
        Cursor.visible = false;
        _playerInput.actions.Enable();
        _playerInput.SwitchCurrentActionMap("Player");
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
