using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RotateCamera : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float mouseX;
    [SerializeField] private float mouseY;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mouseX = Mouse.current.delta.x.ReadValue();
        mouseY = Mouse.current.delta.y.ReadValue();

        transform.position += new Vector3(0f, mouseY * Time.deltaTime, 0f);

        transform.RotateAround()

    }
}
