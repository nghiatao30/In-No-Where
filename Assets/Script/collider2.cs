using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collider2 : MonoBehaviour
{

    public GameObject tl;
    private void OnCollisionEnter(Collision collision)
    {
        tl.SetActive(true);
    }
}
