using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static UnityEditor.Experimental.GraphView.GraphView;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    PlayerInput playerInput;
    CharacterController characterController;

    Vector2 currentMovementInput;
    Vector3 currentMovement;
    Vector3 currentRunMovement;
    bool isMovementPressed;
    bool isRunPressed;

    


    [Header("Settings")]

    [Tooltip("The speed at which the player walks")]
    public float moveSpeed = 4f;
    [Tooltip("The speed at which the player runs")]
    public float runSpeed = 6f;
    [Tooltip("The speed at which the player rotates")]
    public float rotateSpeed = 100f;

    [Tooltip("The power at which the player jumps")]
    public float jumpPower = 8f;
    [Tooltip("The gravity of the evironmetn")]
    [SerializeField] private float gravity = -9.81f;

    [Header("Jump Timing")]
    public float jumpTimeLeniency = 0.1f;
    float timeToStopLeninent;
    

    [SerializeField]
    private float horizontalInput;
    [SerializeField]
    private float verticalInput;

    [SerializeField] private float landAnimDuration = 0.1f;

    [SerializeField] private float toggleJump = 0;
    [SerializeField] private bool isJumpPressed = false;
    [SerializeField] private float velocityY;
    [SerializeField] private float initialJumpVelocity = 2f;
    [SerializeField] private float jumpAnimDuration = 0.4f;
    [SerializeField] private float maxJumpHeight = 1.8f;
    [SerializeField] private float maxJumpTime = 2f;
    [SerializeField] private float groundedGravity = -.05f;

    [SerializeField] private float fallingOffset = 0.5f;
    private float fallingTime;

    private Animator anim;
    int isRunningHash;
    int isWalkingHash;




    private Rigidbody rb;


    public LayerMask groundMask;
    [SerializeField] bool isGrounded = false;

    public bool IsGrounded   // property
    {
        get { return isGrounded; }   // get method
        set { isGrounded = value; }  // set method
    }


    //private void Awake()
    //{
    //    setUpJumpVariable();
    //}
    void Awake()
    {   
        playerInput = new PlayerInput();
        characterController = GetComponent<CharacterController>();

        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");


        playerInput.CharacterControls.Move.started += OnMoveMentInput;
        playerInput.CharacterControls.Move.canceled += OnMoveMentInput;
        playerInput.CharacterControls.Move.performed += OnMoveMentInput;
        playerInput.CharacterControls.Run.started += OnRun;
        playerInput.CharacterControls.Run.canceled += OnRun;

    }

    void OnMoveMentInput(InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<Vector2>();
        currentMovement.x = currentMovementInput.x * moveSpeed;
        currentMovement.z = currentMovementInput.y * moveSpeed;
        currentRunMovement.x = currentMovementInput.x * runSpeed;
        currentRunMovement.z = currentMovementInput.y * runSpeed;
        isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;
    }

    void OnRun(InputAction.CallbackContext context)
    {
        isRunPressed = context.ReadValueAsButton();
    }

    void handleAnimation()
    {
        bool isWalking = anim.GetBool(isWalkingHash);
        bool isRunning = anim.GetBool(isRunningHash);

        if(isMovementPressed && !isWalking) {
            anim.SetBool(isWalkingHash, true);
        }

        if (!isMovementPressed && isWalking)
        {
            anim.SetBool(isWalkingHash, false);
        }

        if ((isMovementPressed && isRunPressed) && !isRunning)
        {
            anim.SetBool(isRunningHash, true);

        }
        else if ((!isMovementPressed && !isRunPressed) && isRunning)
        {
            anim.SetBool(isRunningHash, false);
        }
    }

    void handleGravity()
    {
        if(characterController.isGrounded)
        {
            currentMovement.y = groundedGravity;
            currentRunMovement.y = groundedGravity;
        }
        else
        {
            currentMovement.y = gravity;
            currentRunMovement.y = gravity;
        }
    }
    void Update()
    {
        handleAnimation();

        if (isRunPressed)
        {
            characterController.Move(currentRunMovement * Time.deltaTime);
        }
        else
        {
            characterController.Move(currentMovement * Time.deltaTime);

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

    //private void FixedUpdate()
    //{
    //    jumpingProcess();
    //}



    #region Cached Properties

    private int currentState;

    private static readonly int Idle = Animator.StringToHash("Idle");
    private static readonly int walkForward = Animator.StringToHash("Walk Forwards");
    private static readonly int runForward = Animator.StringToHash("Run Forwards");
    private static readonly int Jump = Animator.StringToHash("Jump");
    private static readonly int Fall = Animator.StringToHash("Fall");
    //private static readonly int Land = Animator.StringToHash("Land");
    //private static readonly int Attack = Animator.StringToHash("Attack");
    //private static readonly int Crouch = Animator.StringToHash("Crouch");

    #endregion
}
