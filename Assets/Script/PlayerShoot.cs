using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    PlayerInput playerInput;
    public static Action shootInput;
    public static Action reloadInput;

    [SerializeField] private KeyCode reloadKey = KeyCode.R;

    private void Awake()
    {
        playerInput = new PlayerInput();
        playerInput.CharacterControls.Shoot.started += OnShoot;
        playerInput.CharacterControls.Shoot.canceled += OnShoot;
        playerInput.CharacterControls.Reload.started += OnReload;
        playerInput.CharacterControls.Reload.canceled += OnReload;

    }

    void OnShoot(InputAction.CallbackContext context)
    {
        if(context.ReadValueAsButton())
            {
            shootInput.Invoke();
        }
        
    }

    void OnReload(InputAction.CallbackContext context)
    {
        if (context.ReadValueAsButton())
        {
            reloadInput.Invoke();
        }
    }
    void OnEnable()
    {
        playerInput.CharacterControls.Enable();
    }

    void OnDisable()
    {
        playerInput.CharacterControls.Disable();
    }
}