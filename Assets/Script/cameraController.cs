using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;


/// <summary>
/// This class uses processed input from the input manager to control the vertical rotation of the camera
/// </summary>
public class CameraController : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("The camera to have control over")]
    public Camera controledCamera;
    [Tooltip("The speed at which the camera rotates to look up and down (calculated in degrees)")]
    public float rotationSpeed = 10f;
    [Tooltip("Whether or not to invert the look direction")]
    public bool invert = true;

    CinemachineFreeLook cinemachineFreeLook;

    [SerializeField] private float mouseX;
    [SerializeField] private float mouseY;
    

    // The input manager to read input from

    /// <summary>
    /// Description:
    /// Standard Unity function called once before the first Update call
    /// Input:
    /// none
    /// Return:
    /// void (no return)
    /// </summary>
    void Start()
    {   
        SetUpCamera();
        cinemachineFreeLook = GetComponent<CinemachineFreeLook>();
    }

    // Wait this many frames before starting to process the camera rotation
    int waitForFrames = 3;
    int framesWaited = 0;

    /// <summary>
    /// Description:
    /// Standard Unity function called once every frame
    /// Input:
    /// none
    /// Return:
    /// void (no return)
    /// </summary>
    void Update()
    {
        // Wait so many frames to avoid startup camera movement bug
        //if (framesWaited <= waitForFrames)
        //{
        //    framesWaited += 1;
        //    return;
        //}
        //ProcessRotation();
        mouseX = Mouse.current.delta.x.ReadValue();
        mouseY = Mouse.current.delta.y.ReadValue();
        





    }

    /// <summary>
    /// Description:
    /// Sets up the camera component if not already donw
    /// Input:
    /// none
    /// Return:
    /// void (no return)
    /// </summary>
    void SetUpCamera()
    {
        if (controledCamera == null)
        {
            controledCamera = GetComponent<Camera>();
        }
    }

    /// <summary>
    /// Description:
    /// Gets the input manager from the scene
    /// Input:
    /// none
    /// Return:
    /// void (no return)
    /// </summary>
    /// <summary>
    /// Description:
    /// Process the vertical look input to rotate the player accordingly
    /// Input:
    /// none
    /// Return:
    /// void (no return)
    /// </summary>
    void ProcessRotation()
    {
        //float verticalLookInput = Input.GetAxis("Vertical");
        //Vector3 cameraRotation = controledCamera.transform.rotation.eulerAngles;
        //float newXRotation = cameraRotation.x + verticalLookInput * rotationSpeed * Time.deltaTime;
        //// clamp the rotation 360 - 270 is up 0 - 90 is down
        //// Because of the way eular angles work with Unity's rotations we have to act differently when clamping the rotation
        //if (newXRotation < 270 && newXRotation >= 180)
        //{
        //    newXRotation = 270;
        //}
        //else if (newXRotation > 45 && newXRotation < 180)
        //{
        //    newXRotation = 45;
        //}
        //controledCamera.transform.rotation = Quaternion.Euler(new Vector3(newXRotation, cameraRotation.y, cameraRotation.z));




    }
}


