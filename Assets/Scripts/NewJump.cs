using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewJump : MonoBehaviour
{
    private Rigidbody2D neo;
    private Animator anim;

    public LayerMask mask;
    public float boxHeight;
    public float jumpValue;

    private Vector2 playerSize;
    private Vector2 boxSize;

    public  float fallMulti = 2.5f;
    public float lowJumpMulti = 2f;
    private bool jumpRequest = false;
    private bool isGround = false;
    
    // Start is called before the first frame update
    void Start()
    {
        neo = GetComponent<Rigidbody2D>();
        playerSize = GetComponent<SpriteRenderer>().bounds.size;
        boxSize = new Vector2(playerSize.x * 0.0f, boxHeight);
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump") && isGround)
        {
            jumpRequest = true;
        }
        SwitchAnim();
    }

    private void FixedUpdate()
    {
        if (jumpRequest)
        {
            neo.AddForce(Vector2.up * jumpValue, ForceMode2D.Impulse);
            jumpRequest = false;
        }
        else
        {
            Vector2 boxCenter = (Vector2) transform.position + (Vector2.down * playerSize.y * 0.5f);

            if (Physics2D.OverlapBox(boxCenter, boxSize, 0, mask) != null)
            {
                isGround = true;
            }
            else
            {
                isGround = false;
            }
        }
        
        if (neo.velocity.y < 0)
        {
            neo.gravityScale = fallMulti;
        } else if (neo.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            neo.gravityScale = lowJumpMulti;
        }
        else
        {
            neo.gravityScale = 10f;
        }
    }

    void SwitchAnim()
    {
        anim.SetFloat("running", Mathf.Abs(neo.velocity.x));

        if (isGround)
        {
            anim.SetBool("falling", false);
            anim.SetBool("jumping", false);
        }
        else if (!isGround && neo.velocity.y > 0)
        {
            anim.SetBool("jumping", true);
            anim.SetBool("falling", false);
        }
        else if (neo.velocity.y < 0)
        {
            anim.SetBool("jumping", false);
            anim.SetBool("falling", true);
        }
    }
}
