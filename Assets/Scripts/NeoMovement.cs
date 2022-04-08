using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeoMovement : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    private Rigidbody2D neo;
    private Animator anim;

    public LayerMask mask;
    public float boxHeight = 0.05f;
    public float jumpValue = 60f;
    public float speed = 20f;
    public float fallMulti = 20f;
    public float lowJumpMulti = 25f;
    public int jumpCount = 1;
    public bool isJump = false;
    public static bool isGetWeapon = false;
    public static bool isGetSkill = false;
    public static int healthPoint = 3;
    private Vector2 playerSize;
    private Vector2 boxSize;
    private float horizontalMove;
    private bool jumpRequest = false;
    private bool isGround = false;
    public GameObject ant;
    private NeoState neoState;
    // private bool attack = false;
    
    // Start is called before the first frame update
    void Start()
    {
        ant = GameObject.Find("Neo");
        neoState = ant.GetComponent<NeoState>();
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

        if (jumpCount <= 0)
        {
            isGetSkill = false;
        }
        if (isGround && jumpCount <= 0)
        {
            jumpCount = 1;
        }

        if (isGround && isGetSkill)
        {
            jumpCount = 2;
        }
        
        
        SwitchAnim();
    }

    private void FixedUpdate()
    {
        // Debug.Log();
        if (jumpRequest)
        {
            neo.AddForce(Vector2.up * jumpValue, ForceMode2D.Impulse);
            jumpRequest = false;
            isGround = false;
            jumpCount -= 1;
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
            if (Input.GetButtonDown("Jump") && isGetSkill)
            {
                neo.AddForce(Vector2.up * jumpValue, ForceMode2D.Impulse);
            }
        } else if (neo.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            neo.gravityScale = lowJumpMulti;
        }
        else
        {
            neo.gravityScale = 10f;
        }
        if(Input.GetKey(KeyCode.J)){
            Shoot();
        }
    }
    public bool facingRight = true;
    private void Run()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal");
        neo.velocity = new Vector2(horizontalMove * speed, neo.velocity.y);
        
        if (horizontalMove != 0)
        {
            if(horizontalMove>0){
                facingRight = true;
            }else{
                facingRight = false;
            }
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
            isGetSkill = true;
            col.GetComponent<Animator>().SetTrigger("get");
            Destroy(col.gameObject, 1.5f);
        }

        if (col.gameObject.name == "Minion")
        {
            neoState.TakeDamage(1);
            if (neoState.currentPlayerState==PlayerState.Die)
            {
                gameObject.SetActive(false);
                Destroy(GameObject.Find("sword"));
                // die
            }
            else
            {
                // invincibility
            }
        }
        if(col.gameObject.name=="TheBoss"){
            neoState.TakeDamage(1);
            if (neoState.currentPlayerState==PlayerState.Die)
            {
                gameObject.SetActive(false);
                Destroy(GameObject.Find("sword"));
                // die
            }
        }
        if(col.gameObject.name=="FlockingMinion"){
            neoState.TakeDamage(1);
            if (neoState.currentPlayerState==PlayerState.Die)
            {
                gameObject.SetActive(false);
                Destroy(GameObject.Find("sword"));
                // die
            }
        }
    }
    void Shoot(){
        GameObject bullet = Instantiate(bulletPrefab,firePoint.position,firePoint.rotation);
        Destroy(bullet,3f);
    }

}
