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
     
    [SerializeField] private bool isJumpPressed = false;
    [SerializeField] private Vector3 currentJumpVelocity = new Vector3 (0,0,0);
    [SerializeField] private float initialJumpVelocity = 2f;
    [SerializeField] private float jumpAnimDuration = 0.4f;
    [SerializeField] private float maxJumpHeight = 1f;
    [SerializeField] private float maxJumpTime = 2f;
    [SerializeField] private float groundedGravity = -.05f;


    [SerializeField] private float attackAnimTime = 0.2f;
    [SerializeField] private float fallingOffset = 0.5f;
    private float fallingTime;

    private Animator anim;
    private Rigidbody rb;

    public LayerMask groundMask;
    BoxCollider boxCollider;
    float maxGroundDist;
    [SerializeField]  float raycastYOffset = 2f;

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


    private void Awake()
    {
        setUpJumpVariable();
    }
    void Start()
    {   
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        groundMask = 8;
        boxCollider = GetComponent<BoxCollider>();
        maxGroundDist = boxCollider.bounds.size.y + 0.01f;
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
        if (transform.position.y < 0)
        {
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        }

        if (isGrounded)
        {
            isJumping = false;
            isFalling = false;
        }

        isWalking = false;
        isRunning = false;

        horizontalInput = Input.GetAxis("Horizontal");
        //verticalInput = Input.GetAxis("Vertical");
        checkJumpressed();
        moveProcess();
        rotatingProcess();
        //handleGravity();
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

        if (isGrounded)
        {
            //if (rb.velocity.x > 0 || rb.velocity.z > 0) return walkForward;
            if (isWalking) return walkForward;
            if (isRunning) return runForward;
            return Idle;
        }
        else if (isJumping) return Jump;
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

    void handleGravity()
    {
        if (isGrounded) {
            transform.position = new Vector3(transform.position.x, groundedGravity, transform.position.z);
        }
        else
        {   
            transform.position += Vector3.Normalize(Vector3.up) * gravity * Time.deltaTime;
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
        if(isGrounded && Input.GetKeyDown("space")) {
            isJumping = true;
            //currentJumpVelocity.y = initialJumpVelocity;
            //transform.position += currentJumpVelocity * Time.deltaTime; 
            transform.position += Vector3.up * jumpPower * Time.deltaTime;
        }
        //else if (!isJumpPressed && isJumping && isGrounded)
        //{ 
        //    isJumping = false;
        //}
    }

    void jumpingProcess()
    {   
        if (Input.GetKeyDown("space") && isGrounded)
        {   
            isJumping = true;
            rb.AddForce(Vector3.Normalize(Vector3.up) * jumpPower, ForceMode.Impulse); 
        }
    }


    void rotatingProcess()
    {
        Vector3 playerRotate = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(new Vector3(playerRotate.x, playerRotate.y + horizontalInput * rotateSpeed * Time.deltaTime, playerRotate.z));
    }
}
