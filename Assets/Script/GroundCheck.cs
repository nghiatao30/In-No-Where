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
                Debug.Log("Player is on ground.");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            // Find the PlayerController component in the scene
            PlayerController playerController = FindObjectOfType<PlayerController>();

            // Check if PlayerController component is found
            if (playerController != null)
            {
                // Set the IsGrounded attribute to true
                playerController.IsGrounded = false;
                Debug.Log("Player not on ground.");
            }
        }
    }
}
