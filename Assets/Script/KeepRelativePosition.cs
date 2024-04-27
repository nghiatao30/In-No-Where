using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepRelativePosition : MonoBehaviour
{
    public Transform player;
    void Start()
    {

    }

    void Update()
    {
        // Set the rotation of the GameObject back to its initial rotation
        transform.SetParent(player, false);
    }
}
