using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

public class IsAimingState : MonoBehaviour
{
    // Start is called before the first frame update
    private AimPlayerController aimPlayerController;
    private PlayerController playerController;
    [SerializeField] GameObject aimingCamera;
    [SerializeField] Rig aimingRig;
    [SerializeField] Rig idleRig;
    PlayerInput playerInput;
    bool isAiming = false;
    float rigWeight;
    float idleWeight;
    float aimingLayerWeight;
    //Animator anim;

    Animator aimingAnimationLayer;

    void Awake()
    {   
        //anim = GetComponent<Animator>();
        aimingAnimationLayer = GetComponent<Animator>();
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
            rigWeight = 1f;
            idleWeight = 0f;
            aimingLayerWeight = 1f;
        }
        else
        {
            aimPlayerController.enabled = false;
            playerController.enabled = true;
            aimingCamera.SetActive(false);
            rigWeight = 0f;
            idleWeight = 1f;
            aimingLayerWeight = 0f;
        }

        aimingRig.weight = Mathf.Lerp(aimingRig.weight, rigWeight, Time.deltaTime * 20f);
        idleRig.weight = Mathf.Lerp(idleRig.weight, idleWeight, Time.deltaTime * 20f);
        aimingAnimationLayer.SetLayerWeight(2, Mathf.Lerp(aimingAnimationLayer.GetLayerWeight(2), aimingLayerWeight, Time.deltaTime * 20f));
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
