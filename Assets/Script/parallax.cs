using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using DG.Tweening;
public class parallax : MonoBehaviour
{
    public float length, startPos;
    public GameObject camera;
    public float parallaxEffect;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<TilemapRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float temp = camera.transform.position.x * (1 - parallaxEffect);
        float dist = camera.transform.position.x * parallaxEffect;
        transform.position = new Vector2(startPos + dist, transform.position.y);

        if(temp > startPos + length) { 
            startPos += length; 
        }
        if (temp < startPos - length) startPos -= length;
    }
}
