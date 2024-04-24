using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class RotateCamera : MonoBehaviour
{
    // Start is called before the first frame update
    public float horizontalSpeed = 4.0f; // Horizontal rotation speed
    public float verticalSpeed = 4.0f;   // Vertical rotation speed

    public Transform targetObject;
    public float degreesRate = 5f;

    private float yaw = 0.0f;    // Current yaw rotation
    private float pitch = 0.0f;  // Current pitch rotation
    float mouseX;
    float mouseY;

    void Update()
    {
        mouseX = Mouse.current.delta.x.ReadValue();
        mouseY = Mouse.current.delta.y.ReadValue();
        // Get mouse input
        rotateCam();
        rotateAroundPlayer();

    }

    void rotateCam()
    {
        

        // Calculate yaw (horizontal) rotation
        yaw += horizontalSpeed * mouseX * Time.deltaTime;

        // Calculate pitch (vertical) rotation
        pitch -= verticalSpeed * mouseY * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, -45f, 45f); // Clamp pitch to avoid flipping

        // Apply rotations to the object
        transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
    }


    void rotateAroundPlayer()
    {   
        transform.RotateAround(targetObject.transform.position, Vector3.up, mouseX * degreesRate * Time.deltaTime);
    }
}

