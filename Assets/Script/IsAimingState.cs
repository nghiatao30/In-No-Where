using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class IsAimingState : MonoBehaviour
{
    // Start is called before the first frame update
    private AimPlayerController aimPlayerController;
    private PlayerController playerController;
    [SerializeField] GameObject aimingCamera;
    PlayerInput playerInput;
    bool isAiming;
    void Start()
    {
        aimPlayerController = GetComponent<AimPlayerController>();
        playerController = GetComponent<PlayerController>();
        playerInput = new PlayerInput();

        playerInput.CharacterControls.Aiming.started += OnAiming;
        playerInput.CharacterControls.Aiming.canceled += OnAiming;
    }

    void OnAiming(InputAction.CallbackContext context)
    {
        isAiming = context.ReadValueAsButton();
    }
    // Update is called once per frame
    void Update()
    {
        if(isAiming)
        {
            aimPlayerController.enabled = true;
            playerController.enabled = false;
            aimingCamera.SetActive(true);
        }
        else
        {
            aimPlayerController.enabled = false;
            playerController.enabled = true;
            aimingCamera.SetActive(false);
        }
    }
}
