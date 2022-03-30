using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeoMovement : MonoBehaviour
{
    private Rigidbody2D neo;

    public float speed;

    private float horizontalMove;
    private bool isJumpPress;
    
    // Start is called before the first frame update
    void Start()
    {
        neo = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Run();
    }

    // void FixedUpdate()
    // {
    //     Run();
    // }

    private void Run()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal");
        neo.velocity = new Vector2(horizontalMove * speed, neo.velocity.y);
        
        if (horizontalMove != 0)
        {
            transform.localScale = new Vector3(horizontalMove, 1, 1);
        }
    }
    
}
