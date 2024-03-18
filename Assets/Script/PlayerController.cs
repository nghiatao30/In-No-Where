using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static UnityEditor.Experimental.GraphView.GraphView;


public class PlayerController : MonoBehaviour
{


    [Header("Settings")]

    [Tooltip("The speed at which the player walks")]
    public float moveSpeed = 4f;
    [Tooltip("The speed at which the player rotates")]
    public float rotateSpeed = 60f;

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
    private Rigidbody rb;


    public LayerMask groundMask;
    BoxCollider boxCollider;
    float maxGroundDist;
    [SerializeField]  float raycastYOffset = 2f;
    [SerializeField] private Transform groundCheck;

    [SerializeField] bool isRunning;
    [SerializeField] bool isWalking;
    [SerializeField] bool isGrounded = false;
    [SerializeField] bool isJumping;
    [SerializeField] bool isFalling;
    [SerializeField] bool isLanding;
    [SerializeField] float lockedTill;

    public bool IsGrounded   // property
    {
        get { return isGrounded; }   // get method
        set { isGrounded = value; }  // set method
    }


    //private void Awake()
    //{
    //    setUpJumpVariable();
    //}
    void Start()
    {   
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        //RaycastHit hit;
        //Ray ray = new Ray(new Vector3(transform.position.x, transform.position.y, transform.position.z), -Vector3.up);
        //if (Physics.Raycast(ray, out hit, raycastYOffset))
        //{
        //    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * raycastYOffset, Color.yellow);
        //    Debug.Log("player is on ground");
        //    isGrounded = true;
        //    isJumping = false;
        //    isFalling = false;
        //}
        //else
        //{
        //    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * raycastYOffset, Color.white);
        //    Debug.Log("not on ground");
        //    isGrounded = false;
        //}

        isGrounded = IsGround();
        Debug.Log(isGrounded);

        
        
        if (isGrounded)
        {
            gravity = 0f;
            velocityY = 0f;
            isJumping = false;
            isFalling = false;
        }
        else
        {
            gravity = Physics.gravity.y;
        }


        isWalking = false;
        isRunning = false;

        horizontalInput = Input.GetAxis("Horizontal");
        //verticalInput = Input.GetAxis("Vertical");
        checkJumpressed();
        moveProcess();
        rotatingProcess();
        handleJump();



        var state = GetState();
        if (state == currentState) return;
        anim.CrossFade(state, 0, 0);
        currentState = state;

        /*heckJumpressed();*/
        

        

    }


    //private void FixedUpdate()
    //{
    //    jumpingProcess();
    //}

    private int GetState()
    {
        if (Time.time < lockedTill) return currentState;

        // Priorities
        //if (_attacked) return LockState(Attack, _attackAnimTime);
        //if (_player.Crouching) return Crouch;
        //if (isLanding) return LockState(Land, landAnimDuration);

        //if (isGrounded) return OnGround.x == 0 ? Idle : Walk;
        //else return OnAir.y > 0 ? Jump : Fall;

        if (isJumping)
        {
            toggleJump = 1;
            return Jump;
        }
        else if (isGrounded)
        {
            if (isWalking) return walkForward;
            if (isRunning) return runForward;
            return Idle;
        }
        else
        {
            if (!isFalling)
            {
                fallingTime = Time.time + fallingOffset;
                isFalling = true;
            }
            else return Time.time > fallingTime ? Fall : currentState;
        }
            int LockState(int s, float t)
        {
            lockedTill = Time.time + t;
            return s;
        }
        return currentState;


    }

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
    void moveProcess()
    {
        if (Input.GetKey("w"))
            {
            transform.position += transform.TransformDirection(Vector3.forward * moveSpeed * Time.deltaTime);
            checkMoving();
        }
        if (Input.GetKey("a"))
        {
            transform.position += transform.TransformDirection(Vector3.left * moveSpeed * Time.deltaTime);
            checkMoving();
        }
        if (Input.GetKey("d"))
        {
            transform.position += transform.TransformDirection(Vector3.right * moveSpeed * Time.deltaTime);
            checkMoving();
        }
        if (Input.GetKey("s"))
        {
            transform.position += transform.TransformDirection(Vector3.back * moveSpeed * Time.deltaTime);
            checkMoving();
        }
    }

    void checkJumpressed()
    { 
        if(Input.GetKeyDown(KeyCode.Space)) {
            isJumpPressed = true;
        }
        else if(Input.GetKeyUp(KeyCode.Space))
        {
            isJumpPressed = false;
        }
    }



    void checkMoving()
    {
        if (isGrounded)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                isRunning = true;
                moveSpeed = 6.5f;
            }
            else
            {
                isWalking = true;
                moveSpeed = 4f;
            }
        }
    }

    void setUpJumpVariable()
    {
        float timeToApex = maxJumpTime / 2;
        gravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        initialJumpVelocity = (2 * maxJumpHeight) / timeToApex;
    }

    void handleJump()
    {
        toggleJump = 0;
        if (isGrounded && Input.GetKeyDown("space") && toggleJump == 0)
        {   
            isJumping = true;
            isFalling = false;
            isWalking = false;
            isRunning = false;
            //currentJumpVelocity.y = initialJumpVelocity;
            //transform.position += currentJumpVelocity * Time.deltaTime; 
            velocityY = Mathf.Sqrt(maxJumpHeight * -2 * Physics.gravity.y);
        }
        velocityY += gravity * Time.deltaTime;
        transform.Translate(new Vector3(0, velocityY, 0) * Time.deltaTime);
    }

    void rotatingProcess()
    {
        Vector3 playerRotate = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(new Vector3(playerRotate.x, playerRotate.y + horizontalInput * rotateSpeed * Time.deltaTime, playerRotate.z));
    }

    bool IsGround()
    {
        return Physics.CheckSphere(groundCheck.position, .1f, groundMask);
    }
}
