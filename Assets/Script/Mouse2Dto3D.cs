using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Mouse2Dto3D : MonoBehaviour
{
    public Camera mainCam;
    public LayerMask mouseColliderLayerMask;
    [SerializeField] float maxDis;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   
        Ray ray = mainCam.ScreenPointToRay( Input.mousePosition );
        if(Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            transform.position = hitInfo.point;
        }
        
    }
}
