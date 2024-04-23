using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class RotateCamera : MonoBehaviour
{
    // Start is called before the first frame update
    public float horizontalSpeed = 2.0f; // Horizontal rotation speed
    public float verticalSpeed = 2.0f;   // Vertical rotation speed

    private float yaw = 0.0f;    // Current yaw rotation
    private float pitch = 0.0f;  // Current pitch rotation

    void Update()
    {
        // Get mouse input
        float mouseX = Mouse.current.delta.x.ReadValue();
        float mouseY = Mouse.current.delta.y.ReadValue();

        // Calculate yaw (horizontal) rotation
        yaw += horizontalSpeed * mouseX * Time.deltaTime;

        // Calculate pitch (vertical) rotation
        pitch -= verticalSpeed * mouseY * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, -90f, 90f); // Clamp pitch to avoid flipping

        // Apply rotations to the object
        transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
    }
}

