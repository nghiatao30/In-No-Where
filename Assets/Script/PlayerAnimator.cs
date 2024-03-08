using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class PlayerAnimator : MonoBehaviour
{

    private PlayerController playerController;
    private Animator anim;

    bool isRunning;
    bool isGrounded;
    bool isJumping;
    bool isWalking;
    bool isLanding;
    float lockedTill;

    void Awake()
    {
        if (!TryGetComponent(out PlayerController player))
        {
            Destroy(this);
            return;
        }

        playerController = player;
        anim = GetComponent<Animator>();
    }


    void Start()
    {
        
    }


    void Update()
    {
        
    }
}
