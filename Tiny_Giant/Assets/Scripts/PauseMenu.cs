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
    private PlayerInput _playerInput;

    [SerializeField] private GameObject pauseUI;
    private bool isPaused;

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
        _pauseMenu = _inputActions.UI.Escape;
        _pauseMenu.Enable();
        _pauseMenu.performed += Pause;
    }

    private void OnDisable()
    {
        _pauseMenu.Disable();
    }

    void Pause(InputAction.CallbackContext context)
    {
        if (!_playerInput) _playerInput = GameObject.FindWithTag("Player").GetComponent<PlayerInput>();
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
        pauseUI.SetActive(true);
        Cursor.visible = true;
        _playerInput.actions.Disable();
        Cursor.lockState = CursorLockMode.None;
    }

    public void DeactivatePauseMenu()
    {
        pauseUI.SetActive(false);
        Cursor.visible = false;
        _playerInput.actions.Enable();
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
