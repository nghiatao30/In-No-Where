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
    public GameObject player;
    PlayerInput playerInput;
    CharacterController characterController;
    GameObject virCam;
    [SerializeField] private Transform cameraDirection;

    Vector2 currentMovementInput;
    Vector3 currentMovement;
    Vector3 currentRunMovement;
    bool isMovementPressed;
    bool isRunPressed;
    bool isJumping;

    public Transform cameraCenter;
    public Vector3 offset;

    [Header("Settings")]

    [Tooltip("The speed at which the player walks")]
    public float moveSpeed = 4f;
    [Tooltip("The speed at which the player runs")]
    public float runSpeed = 8f;
    [Tooltip("The frame rate at which the player rotates")]
    public float rotationPerFrame = 15f;

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
    [SerializeField] private float fallMultiplier = 2f;
    [SerializeField] private float fallingOffset = 0.5f;
    private float fallingTime;

    private Animator anim;
    int isRunningHash;
    int isWalkingHash;
    int isJumpingHash;
    bool isJumpAnimating;
    int jumpCount = 0;
    Dictionary<int, float> initialJumpVelocities = new Dictionary<int, float>();
    Dictionary<int, float> jumpGravities = new Dictionary<int, float>();



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
        characterController = player.GetComponent<CharacterController>();
        cameraDirection =  GameObject.Find("VirtualCamera1").transform; 


        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        isJumpingHash = Animator.StringToHash("isJumped");


        playerInput.CharacterControls.Move.started += OnMoveMentInput;
        playerInput.CharacterControls.Move.canceled += OnMoveMentInput;
        playerInput.CharacterControls.Move.performed += OnMoveMentInput;
        playerInput.CharacterControls.Run.started += OnRun;
        playerInput.CharacterControls.Run.canceled += OnRun;
        playerInput.CharacterControls.Jump.started += OnJump;
        playerInput.CharacterControls.Jump.canceled += OnJump;

        SetUpJumpVar();

    }

    void handleRotation()
    {
        Vector3 positionToLookAt;

        positionToLookAt.x = currentMovement.x;
        positionToLookAt.y = 0.0f;
        positionToLookAt.z = currentMovement.z;

        Quaternion currentRotation = transform.rotation;

        positionToLookAt = newDirOnCamAxis(positionToLookAt);

        if(isMovementPressed)
        {
            //if(currentMovement.z != 0.0f)
            //{
            //    Vector3 rotateDir = new Vector3(transform.position.x - cameraDirection.position.x, 0f, transform.position.z - cameraDirection.position.z);
            //    Quaternion toLookRotation = currentMovement.z > 0? Quaternion.LookRotation(rotateDir) : Quaternion.LookRotation(-rotateDir);
            //    transform.rotation = Quaternion.Slerp(currentRotation, toLookRotation, rotationPerFrame * Time.deltaTime);
            //}
            //else
            //{
            //    Quaternion toLookRotation = Quaternion.LookRotation(positionToLookAt);
            //    transform.rotation = Quaternion.Slerp(currentRotation, toLookRotation, rotationPerFrame * Time.deltaTime);
            //}

            Quaternion toLookRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, toLookRotation, rotationPerFrame * Time.deltaTime);


        }

    }
    void SetUpJumpVar()
    {
        float timeToApex = maxJumpTime / 2;
        gravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        initialJumpVelocity = (2 * maxJumpHeight) / timeToApex;
        float secondJumpGravity = (-2 * (maxJumpHeight + 2)) / Mathf.Pow(timeToApex * 1.25f, 2);
        float secondJumpVelocity = (2 * (maxJumpHeight + 2)) / (timeToApex * 1.25f);
        float thirdJumpGravity = (-2 * (maxJumpHeight + 4)) / Mathf.Pow(timeToApex * 1.5f, 2);
        float thirdJumpVelocity = (2 * (maxJumpHeight + 4)) / (timeToApex * 1.5f);

        initialJumpVelocities.Add(1, initialJumpVelocity);
        initialJumpVelocities.Add(2, secondJumpVelocity);
        initialJumpVelocities.Add(3, thirdJumpVelocity);

        jumpGravities.Add(1, gravity);
        jumpGravities.Add(2, secondJumpGravity);
        jumpGravities.Add(3, thirdJumpGravity);
    }

    void handleJump()
    {
        if(!isJumping && characterController.isGrounded && isJumpPressed)
        {
            anim.SetBool("isJumped", true);
            isJumpAnimating = true;
            isJumping = true;
            jumpCount += 1;
            currentMovement.y = initialJumpVelocity * .5f;
            currentRunMovement.y = initialJumpVelocity * .5f;

        }else if(!isJumpPressed && isJumping && characterController.isGrounded)
        {
            isJumping= false;
        }
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

    void OnJump ( InputAction.CallbackContext context)
    {
        isJumpPressed = context.ReadValueAsButton();
    }
    void OnRun(InputAction.CallbackContext context)
    {
        isRunPressed = context.ReadValueAsButton();
    }

    void handleAnimation()
    {
        bool isWalking = anim.GetBool(isWalkingHash);
        bool isRunning = anim.GetBool(isRunningHash);

        if(!isMovementPressed)
        {
            anim.SetBool(isWalkingHash, false);
            anim.SetBool(isRunningHash, false);
        }
    

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
        else if ((isMovementPressed && !isRunPressed) && isRunning)
        {
            anim.SetBool(isRunningHash, false);
        }
    }

    void handleGravity()
    {
        bool isFalling = currentMovement.y <= 0.0f;
        if(characterController.isGrounded)
        {
            if(isJumpAnimating)
            {
                anim.SetBool("isJumped", false);
                isJumpAnimating = false;
            }
            currentMovement.y = groundedGravity;
            currentRunMovement.y = groundedGravity;
        }
        else if (isFalling)
        {
            float previousYVelocity = currentMovement.y;
            float newYVelocity = currentRunMovement.y + (gravity * fallMultiplier * Time.deltaTime);
            float nextYVelocity = (previousYVelocity + newYVelocity) * .5f;
            currentMovement.y = nextYVelocity;
            currentRunMovement.y = nextYVelocity;

        }
        else
        {
            float previousYVelocity = currentMovement.y;
            float newYVelocity = currentRunMovement.y + (gravity * Time.deltaTime);
            float nextYVelocity = (previousYVelocity + newYVelocity) * .5f;
            currentMovement.y = nextYVelocity;
            currentRunMovement.y = nextYVelocity;
        }
    }

    Vector3 newDirOnCamAxis(Vector3 originalVector)
    {

        Vector3 newAxis = (transform.position - cameraDirection.position);
        newAxis.y = 0f;

        newAxis.Normalize();

        // Calculate the rotation from the original axis to the new axis
        Quaternion rotation = Quaternion.FromToRotation(Vector3.forward, newAxis);

        // Transform the vector using the rotation
        Vector3 transformedVector = rotation * originalVector;

        return transformedVector;
    }

    void Update()
    {   
        handleAnimation();
        handleRotation();
        handleGravity();
        handleJump();

        if (isRunPressed)
        {   
            characterController.Move(newDirOnCamAxis(currentRunMovement) * Time.deltaTime);
        }
        else
        {
            characterController.Move(newDirOnCamAxis(currentMovement) * Time.deltaTime);
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
