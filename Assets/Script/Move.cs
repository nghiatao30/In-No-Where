using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Move : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.DOMove(new Vector3(1,2,3),2f).SetEase(Ease.InBounce);
    }

    void Update()
    {
        
    }
}
