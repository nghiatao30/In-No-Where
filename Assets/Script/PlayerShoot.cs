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
    bool isShoot = false;
    bool isReload = false;


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
        isShoot = context.ReadValueAsButton();
    }

    void OnReload(InputAction.CallbackContext context)
    {
        isReload = context.ReadValueAsButton();
    }

    private void Update()
    {
        if(isShoot) shootInput.Invoke();
        if(isReload) reloadInput.Invoke();  

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