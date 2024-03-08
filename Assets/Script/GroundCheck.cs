using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            // Find the PlayerController component in the scene
            PlayerController playerController = FindObjectOfType<PlayerController>();

            // Check if PlayerController component is found
            if (playerController != null)
            {
                // Set the IsGrounded attribute to true
                playerController.IsGrounded = true;
                Debug.Log("PlayerController is on ground.");
            }
            else
            {
                Debug.LogError("PlayerController not found in the scene.");
            }
        }
    }
}
