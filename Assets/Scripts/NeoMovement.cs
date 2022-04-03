using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NeoMovement : MonoBehaviour
{
    private Rigidbody2D neo;
    private Animator anim;

    public LayerMask mask;
    public float boxHeight = 0.05f;
    public float jumpValue = 60f;
    public float speed = 20f;
    public float fallMulti = 20f;
    public float lowJumpMulti = 25f;
    public int jumpCount;
    public int healthPoint = 3;
    public bool isGetWeapon = false;
    public bool isJump = false;
    public bool haveDoubleJumpSkill = false;

    private Vector2 playerSize;
    private Vector2 boxSize;
    private float horizontalMove;
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
        Run();
        if (Input.GetButtonDown("Jump") && (isGround || jumpCount >= 1) )
        {
            jumpRequest = true;
        }

        if (isGround && jumpCount <= 0)
        {
            jumpCount = 1;
            haveDoubleJumpSkill = false;
        }

        if (isGround && haveDoubleJumpSkill)
        {
            jumpCount = 2;
        }
        SwitchAnim();
    }

    private void FixedUpdate()
    {
        if (jumpRequest)
        {
            neo.AddForce(Vector2.up * jumpValue, ForceMode2D.Impulse);
            jumpRequest = false;
            isGround = false;
            jumpCount--;
        }
        else
        {
            Vector2 boxCenter = (Vector2) transform.position + (Vector2.down * playerSize.y * 0.5f);

            if (Physics2D.OverlapBox(boxCenter, boxSize, 0, mask) != null)
            {
                isGround = true;
                isJump = false;
            }
            else
            {
                isGround = false;
                isJump = true;
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

    private void Run()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal");
        neo.velocity = new Vector2(horizontalMove * speed, neo.velocity.y);
        
        if (horizontalMove != 0)
        {
            transform.localScale = new Vector3(horizontalMove, 1, 1);
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

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "WeaponBox" && !isGetWeapon)
        {
            isGetWeapon = true;
            col.GetComponent<Animator>().SetTrigger("get");
            Destroy(col.gameObject, 1.5f);
        }
        
        if (col.tag == "SkillBox" && jumpCount < 2)
        {
            jumpCount++;
            haveDoubleJumpSkill = true;
            col.GetComponent<Animator>().SetTrigger("get");
            Destroy(col.gameObject, 1.5f);
        }

        if (col.tag == "trench")
        {
            // die
            gameObject.SetActive(false);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            // GameManager.PlayerDied();
        }
        
        if (col.tag == "Enemy")
        {
            healthPoint -= 1;
            if (healthPoint == 0)
            {
                // die
            }
            else
            {
                // invincibility
            }
        }
    }
}
