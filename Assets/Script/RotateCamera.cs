using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RotateCamera : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float mouseX;
    [SerializeField] private float mouseY;
    public Transform lookAtTarget;
    CinemachineFreeLook cinemachineFreeLook;
    private float maxHeightLookAt = 1.4f;
    private float minHeightLookAt = 0f;
    private float rotateYSpeed = 4f;
    void Start()
    {
        cinemachineFreeLook = GetComponent<CinemachineFreeLook>();
    }

    // Update is called once per frame
    void Update()
    {


        //lookYProcess();
        lookXProcess();

    }

    void lookYProcess()
    {
        lookAtTarget.position += new Vector3(0f, mouseY * Time.deltaTime, 0f);

        if (lookAtTarget.position.y < minHeightLookAt)
            lookAtTarget.position = new Vector3(lookAtTarget.position.x, minHeightLookAt, lookAtTarget.position.z);
        else if (lookAtTarget.position.y > maxHeightLookAt)
            lookAtTarget.position = new Vector3(lookAtTarget.position.x, maxHeightLookAt, lookAtTarget.position.z);
    }

    void lookXProcess()
    {
        mouseX = Mouse.current.delta.x.ReadValue();
        mouseY = Mouse.current.delta.y.ReadValue();

        cinemachineFreeLook.m_XAxis.Value += mouseX * rotateYSpeed * Time.deltaTime;
    }

}
